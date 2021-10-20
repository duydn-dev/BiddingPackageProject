using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common.Dtos;
using Neac.Common.Dtos.BiddingPackage;
using Neac.Common.Dtos.ProjectDtos;
using Neac.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Repository
{
    public class ProjectFlowRepository : IProjectFlowRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProjectFlowRepository(ILogRepository logRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<ProjectFlow>> CreateAsync(ProjectFlow request)
        {
            try
            {
                request.ProjectFlowId = Guid.NewGuid();
                await _unitOfWork.GetRepository<ProjectFlow>().Add(request);
                await _unitOfWork.SaveAsync();
                return Response<ProjectFlow>.CreateSuccessResponse(request);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<ProjectFlow>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<List<ProjectFlowCurrentDto>>> CurrentState(Guid projectId)
        {
            try
            {
                // lấy ra tổng số state và number docuemnt của project hiện tại
                //var state = await _unitOfWork.GetRepository<BiddingPackage>().GetAll()
                //    .Where(n => n.BiddingPackageProjects.Any(n => n.ProjectId == projectId))
                //    .Include(n => n.Documents)
                //    .Select(n => new { BiddingPackageId = n.BiddingPackageId, DocumentCount = n.Documents.Count })
                //    .ToListAsync();
                var query = await _unitOfWork.GetRepository<BiddingPackage>().GetAll()
                .Include(n => n.BiddingPackageProjects.OrderBy(g => g.Order))
                .Include(n => n.Documents)
                .Where(n => n.BiddingPackageProjects.Any(n => n.ProjectId == projectId))
                .Select(n => new BiddingPackageTotalDocumentDto
                {
                    BiddingPackageId = n.BiddingPackageId,
                    NumberDocument = n.Documents.Count
                }).ToListAsync();

                var data = query.GroupJoin(
                    _unitOfWork.GetRepository<ProjectFlow>().GetAll(),
                    l => l.BiddingPackageId,
                    r => r.BiddingPackageId,
                    (l, r) => new ProjectFlowCurrentDto {
                        BiddingPackageId = l.BiddingPackageId,
                        TotalDocument = l.NumberDocument,
                        CurrentNumberDocument = r.Count()
                    }).ToList();

                // so sánh với số lượng trong bảng flow
                // nếu không có => init

                // nếu nhỏ hơn

                // nếu bằng thì next step
                return Response<List<ProjectFlowCurrentDto>>.CreateSuccessResponse(data);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<ProjectFlowCurrentDto>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<GetListResponseModel<List<ProjectFlow>>>> GetFilterAsync(string filter)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<GetListUserRequestDto>(filter);
                var query =  _unitOfWork.GetRepository<ProjectFlow>()
                    .GetByExpression(n =>
                        n.DocumentNumber.Contains(request.TextSearch) ||
                        n.DocumentAbstract.Contains(request.TextSearch) ||
                        n.RegulationDocument.Contains(request.TextSearch) ||
                        n.PromulgateUnit.Contains(request.TextSearch) ||
                        n.Signer.Contains(request.TextSearch));

                GetListResponseModel<List<ProjectFlow>> responseData = new GetListResponseModel<List<ProjectFlow>>(query.Count(), request.PageSize);
                var result = await query
                    .OrderByDescending(n => n.DocumentNumber)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();

                responseData.Data = result;
                return Response<GetListResponseModel<List<ProjectFlow>>>.CreateSuccessResponse(responseData);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<ProjectFlow>>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<ProjectFlow>> UpdateAsync(ProjectFlow request)
        {
            try
            {
                var projectFlow = await _unitOfWork.GetRepository<ProjectFlow>().GetByExpression(n => n.ProjectFlowId == request.ProjectFlowId).FirstOrDefaultAsync();
                if (projectFlow == null)
                {
                    await _logRepository.ErrorAsync($"Không tìm thấy bản ghi ProjectFlowId = {request.ProjectFlowId}");
                    return Response<ProjectFlow>.CreateErrorResponse(new Exception($"Không tìm thấy bản ghi ProjectFlowId = {request.ProjectFlowId}"));
                }
                var mapped = _mapper.Map<ProjectFlow, ProjectFlow>(request, projectFlow);
                await _unitOfWork.GetRepository<ProjectFlow>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<ProjectFlow>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<ProjectFlow>.CreateErrorResponse(ex);
            }
        }
    }
}
