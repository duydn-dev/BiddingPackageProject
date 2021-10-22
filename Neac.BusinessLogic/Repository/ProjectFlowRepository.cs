using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.IO;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProjectFlowRepository(ILogRepository logRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<Response<ProjectFlow>> CreateAsync()
        {
            try
            {
                var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];
                string filePath = null;
                if (file != null)
                {
                    filePath = await UploadFile(file);
                }
                var projectFlow = JsonConvert.DeserializeObject<ProjectFlowCreateDto>(_httpContextAccessor.HttpContext.Request.Form["projectFlow"].ToString());
                projectFlow.ProjectFlowId = Guid.NewGuid();
                projectFlow.FileUrl = filePath;
                var request = _mapper.Map<ProjectFlowCreateDto, ProjectFlow>(projectFlow);
                await _unitOfWork.GetRepository<ProjectFlow>().Add(request);
                await _unitOfWork.SaveAsync();
                return Response<ProjectFlow>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<ProjectFlow>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<List<ProjectFlowCurrentDto>>> CurrentState(Guid projectId)
        {
            try
            {
                var documents = _unitOfWork.GetRepository<Document>().GetByExpression(n => n.BiddingPackageId == null);
                var comonQuery = _unitOfWork.GetRepository<BiddingPackage>().GetAll()
                  .Include(n => n.BiddingPackageProjects.OrderBy(g => g.Order))
                  .Include(n => n.Documents)
                  .Where(n => n.BiddingPackageProjects.Any(n => n.ProjectId == projectId));

                var query = await _unitOfWork.GetRepository<BiddingPackage>().GetAll()
                .Include(n => n.BiddingPackageProjects.OrderBy(g => g.Order))
                .Include(n => n.Documents)
                .Where(n => n.BiddingPackageProjects.Any(n => n.ProjectId == projectId))
                .Select(n => new BiddingPackageTotalDocumentDto
                {
                    BiddingPackageId = n.BiddingPackageId,
                    NumberDocument = n.Documents.Count + documents.Count()
                })
                .ToListAsync();

                var data = query.GroupJoin(
                    _unitOfWork.GetRepository<ProjectFlow>().GetAll(),
                    l => l.BiddingPackageId,
                    r => r.BiddingPackageId,
                    (l, r) => new ProjectFlowCurrentDto
                    {
                        BiddingPackageId = l.BiddingPackageId,
                        TotalDocument = l.NumberDocument,
                        CurrentNumberDocument = r.Count()
                    }).ToList();
                return Response<List<ProjectFlowCurrentDto>>.CreateSuccessResponse(data);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<ProjectFlowCurrentDto>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<Guid>> CurrentPackageAsync(Guid projectId)
        {
            Guid currentPackageId = new Guid();
            try
            {
                var listPackage = await _unitOfWork.GetRepository<ProjectFlow>()
                    .GetByExpression(n => n.ProjectId == projectId)
                    .ToListAsync();

                if (listPackage.Count <= 0)
                {
                    var package = await _unitOfWork.GetRepository<Project>()
                        .GetByExpression(n => n.ProjectId == projectId)
                        .Include(n => n.BiddingPackageProjects)
                        .SelectMany(n => n.BiddingPackageProjects)
                        .OrderBy(n => n.Order)
                        .FirstOrDefaultAsync();

                    return Response<Guid>.CreateSuccessResponse(package.BiddingPackageId.Value);
                }
                else
                {
                    var package = await _unitOfWork.GetRepository<BiddingPackageProject>().GetByExpression(n => n.ProjectId == projectId).ToArrayAsync();
                    foreach (var item in package)
                    {
                        var countDocument = await _unitOfWork.GetRepository<Document>()
                            .GetByExpression(n => (n.BiddingPackageId == null) || (n.BiddingPackageId == item.BiddingPackageId))
                            .CountAsync();
                        var countCurrentDocument = await _unitOfWork.GetRepository<ProjectFlow>()
                            .GetByExpression(n => n.ProjectId == projectId && n.BiddingPackageId == item.BiddingPackageId)
                            .CountAsync();
                        if (countDocument <= countCurrentDocument)
                        {
                            currentPackageId = item.BiddingPackageId.Value;
                            break;
                        }
                        else
                            continue;

                    }
                    return Response<Guid>.CreateSuccessResponse(currentPackageId);
                }
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<Guid>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<List<ProjectFlow>>> GetFilterAsync(string filter)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<ProjectFlowGetListRequestDto>(filter);
                var query = await _unitOfWork.GetRepository<ProjectFlow>()
                    .GetByExpression(n =>
                        n.ProjectId == request.ProjectId &&
                        n.BiddingPackageId == request.BiddingPackageId
                        ).ToListAsync();
                return Response<List<ProjectFlow>>.CreateSuccessResponse(query);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<ProjectFlow>>.CreateErrorResponse(ex);
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
        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                string newFileName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(file.FileName);
                string filePath = "Uploads\\" + newFileName;
                string vitualPath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);
                using (var stream = new FileStream(vitualPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return filePath;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return "";
            }
        }

        public async Task<string> DownloadAsync(Guid projectFlowId)
        {
            try
            {
                var flow = await _unitOfWork.GetRepository<ProjectFlow>().GetByExpression(n => n.ProjectFlowId == projectFlowId).FirstOrDefaultAsync();
                if (flow == null)
                {
                    await _logRepository.ErrorAsync("không tìm thấy flow");
                    return null;
                }
                return Path.Combine(_hostingEnvironment.WebRootPath, flow.FileUrl);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return null;
            }
        }
    }
}
