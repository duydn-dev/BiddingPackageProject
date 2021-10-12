using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common;
using Neac.Common.Dtos;
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
    public class ProjectRepository : IProjectRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProjectRepository(ILogRepository logRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<GetListResponseModel<List<ProjectGetListDto>>>> GetFilter(string filter)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<GetListUserRequestDto>(filter);
                var query = _unitOfWork.GetRepository<Project>()
                                    .GetAll()
                                    .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.ProjectName.Contains(request.TextSearch));
                query = query.WhereIf(request.Status.HasValue, n => n.CurrentState == request.Status);

                GetListResponseModel<List<ProjectGetListDto>> responseData = new GetListResponseModel<List<ProjectGetListDto>>(query.Count(), request.PageSize);
                var result = await query
                    .OrderByDescending(n => n.ProjectDate)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();

                responseData.Data = _mapper.Map<List<Project>, List<ProjectGetListDto>>(result);
                return Response<GetListResponseModel<List<ProjectGetListDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<ProjectGetListDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<ProjectGetListDto>> GetByIdAsync(Guid projectId)
        {
            try
            {
                var data = await _unitOfWork.GetRepository<Project>().GetByExpression(n => n.ProjectId == projectId)
                    .Include(n => n.BiddingPackageProjects).FirstOrDefaultAsync();
                return Response<ProjectGetListDto>.CreateSuccessResponse(_mapper.Map<Project, ProjectGetListDto>(data));
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<ProjectGetListDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<ProjectGetListDto>> CreateAsync(ProjectGetListDto request)
        {
            try
            {
                var mappped = _mapper.Map<ProjectGetListDto, Project>(request);
                await _unitOfWork.GetRepository<Project>().Add(mappped);
                await _unitOfWork.SaveAsync();
                return Response<ProjectGetListDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<ProjectGetListDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<ProjectGetListDto>> UpdateAsync(ProjectGetListDto request)
        {
            try
            {
                var project = await _unitOfWork.GetRepository<Project>().GetByExpression(n => n.ProjectId == request.ProjectId).FirstOrDefaultAsync();
                var mappped = _mapper.Map<ProjectGetListDto, Project>(request, project);
                await _unitOfWork.GetRepository<Project>().Update(mappped);
                await _unitOfWork.SaveAsync();
                return Response<ProjectGetListDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<ProjectGetListDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> DeleteAsync(Guid projectId)
        {
            try
            {
                await _unitOfWork.GetRepository<Project>().DeleteByExpression(n => n.ProjectId == projectId);
                await _unitOfWork.SaveAsync();
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
    }
}
