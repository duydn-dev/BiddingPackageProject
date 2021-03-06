using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common.Const;
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
                // kiểm tra gói thầu hiện tại đã đủ văn bản chưa
                var projectFlow = JsonConvert.DeserializeObject<ProjectFlowCreateDto>(_httpContextAccessor.HttpContext.Request.Form["projectFlow"].ToString());
                //var countData = await CurrentState(projectFlow.ProjectId.Value);
                //var currentPackageCount = countData.ResponseData.FirstOrDefault(n => n.BiddingPackageId == projectFlow.BiddingPackageId);
                //if(currentPackageCount?.CurrentDocumentCount >= currentPackageCount?.DocumentCount)
                //{
                //    await _logRepository.ErrorAsync("Gói thầu đã đủ văn bản !");
                //    return Response<ProjectFlow>.CreateErrorResponse(new Exception("Gói thầu đã đủ văn bản !"));
                //}

                if (_httpContextAccessor.HttpContext.Request.Form.Files?.Count > 0)
                {
                    var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];
                    string filePath = null;
                    if (file != null)
                    {
                        filePath = await UploadFile(file);
                        projectFlow.FileUrl = filePath;
                    }
                }

                // tìm kiếm package chưa set văn bản 
                //var isUnsetDocument = countData.ResponseData.Count(n => n.DocumentCount == 0);
                //if(isUnsetDocument <= 0)
                //{
                //    // nếu không có thì so sánh tổng đã nhập + 1 và tổng phải nhập văn bản của project hiện tại
                //    var totalDocument = countData.ResponseData.Select(n => n.DocumentCount).Sum();
                //    var totalImported = countData.ResponseData.Select(n => n.CurrentDocumentCount).Sum() + 1;
                //    if(totalDocument == totalImported)
                //    {
                //        var projectInfo = await _unitOfWork.GetRepository<Project>().GetByExpression(n => n.ProjectId == projectFlow.ProjectId).FirstOrDefaultAsync();
                //        projectInfo.CurrentState = ProjectState.Excuted;
                //        await _unitOfWork.GetRepository<Project>().Update(projectInfo);
                //    }
                //}

                projectFlow.ProjectFlowId = Guid.NewGuid();
                projectFlow.ProjectDate = projectFlow.ProjectDate.Value.AddDays(1);
                var request = _mapper.Map<ProjectFlowCreateDto, ProjectFlow>(projectFlow);
                await _unitOfWork.GetRepository<ProjectFlow>().Add(request);
                await _unitOfWork.SaveAsync();

                return Response<ProjectFlow>.CreateSuccessResponse(new ProjectFlow());
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
                // lấy ra số văn bản cần nhập
                var joined = await (from bp in _unitOfWork.GetRepository<BiddingPackageProject>().GetByExpression(n => n.ProjectId == projectId)

                                    join b in _unitOfWork.GetRepository<BiddingPackage>().GetAll() on bp.BiddingPackageId equals b.BiddingPackageId

                                    join ds in _unitOfWork.GetRepository<DocumentSetting>().GetByExpression(n => n.ProjectId == projectId) on b.BiddingPackageId equals ds.BiddingPackageId
                                    into gr from grData in gr.DefaultIfEmpty()

                                    select new { bp.BiddingPackageId, bp.Order, b.BiddingPackageName, grData.DocumentId })
                               .GroupBy(n => new { n.BiddingPackageId, n.Order, n.BiddingPackageName }, (key, documentIds) => new {
                                   key.BiddingPackageId,
                                   key.Order,
                                   key.BiddingPackageName,
                                   DocumentCount = documentIds.Count(g => g.DocumentId.HasValue)
                               })
                              .OrderBy(n => n.Order)
                              .ToListAsync();

                // lấy ra văn bản đã nhập
                var data = joined.GroupJoin(
                        _unitOfWork.GetRepository<ProjectFlow>().GetByExpression(n => n.ProjectId == projectId),
                        left => left.BiddingPackageId,
                        right => right.BiddingPackageId,
                        (key, data) => new ProjectFlowCurrentDto {
                            BiddingPackageId = key.BiddingPackageId,
                            BiddingPackageName = key.BiddingPackageName,
                            Order = key.Order,
                            DocumentCount = key.DocumentCount,
                            CurrentDocumentCount = data.Count()
                        }
                    ).OrderBy(n => n.Order).ToList();

                // lấy ra package hiện tại
                Guid currentPackageId = new Guid();
                int order = 0;
                foreach (var item in data)
                {
                    if (
                        (item.DocumentCount == 0 && item.CurrentDocumentCount == 0) ||
                        (item.CurrentDocumentCount < item.DocumentCount)
                    )
                    {
                        currentPackageId = item.BiddingPackageId.Value;
                        order = item.Order.Value;
                        break;
                    }
                }

                return Response<List<ProjectFlowCurrentDto>>.CreateSuccessResponse(data, new { currentPackageId, order });
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
                    var totalDocumentByProject = await (from bpp in _unitOfWork.GetRepository<BiddingPackageProject>().GetAll()
                                                        join d in _unitOfWork.GetRepository<Document>().GetAll() on bpp.BiddingPackageId equals d.BiddingPackageId into gr
                                                        from grData in gr.DefaultIfEmpty()
                                                        where bpp.ProjectId == projectId
                                                        orderby bpp.Order ascending
                                                        select new
                                                        {
                                                            BiddingPackageId = bpp.BiddingPackageId,
                                                            Order = bpp.Order,
                                                            DocumentId = (grData == null) ? Guid.Empty : grData.DocumentId
                                                        }
                                                ).ToListAsync();

                    var maxOrder = totalDocumentByProject
                        .Where(n => n.Order == totalDocumentByProject.Max(n => n.Order))
                        .GroupBy(n => new { n.BiddingPackageId, n.Order }, (key, value) => new {
                            key.BiddingPackageId,
                            LastPackageDocumentCount = value.Count(g => g.DocumentId != Guid.Empty)
                        }).FirstOrDefault();

                    var package = await _unitOfWork.GetRepository<BiddingPackageProject>().GetByExpression(n => n.ProjectId == projectId).OrderBy(n => n.Order).ToArrayAsync();
                    foreach (var item in package)
                    {
                        var countDocument = await _unitOfWork.GetRepository<Document>()
                            .GetByExpression(n => (n.BiddingPackageId == null) || (n.BiddingPackageId == item.BiddingPackageId))
                            .CountAsync();
                        var countCurrentDocument = await _unitOfWork.GetRepository<ProjectFlow>()
                            .GetByExpression(n => n.ProjectId == projectId && n.BiddingPackageId == item.BiddingPackageId)
                            .CountAsync();
                        if (countCurrentDocument == countDocument && maxOrder.BiddingPackageId != item.BiddingPackageId)
                        {
                            continue;
                        }
                        else if((countCurrentDocument < countDocument) || (countCurrentDocument == countDocument && maxOrder.BiddingPackageId == item.BiddingPackageId))
                        {
                            currentPackageId = item.BiddingPackageId.Value;
                            break;
                        }
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

        public async Task<Response<List<ProjectFlowGetListDto>>> GetFilterAsync(string filter)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<ProjectFlowGetListRequestDto>(filter);
                var query = await (from pl in _unitOfWork.GetRepository<ProjectFlow>().GetAll()
                               join d in _unitOfWork.GetRepository<Document>().GetAll() on pl.DocumentId equals d.DocumentId
                               where pl.ProjectId == request.ProjectId && pl.BiddingPackageId == request.BiddingPackageId
                               select new ProjectFlowGetListDto
                               {
                                   BiddingPackageId = pl.BiddingPackageId,
                                   ProjectId = pl.ProjectId,
                                   DocumentId = pl.DocumentId,
                                   DocumentAbstract = pl.DocumentAbstract,
                                   DocumentName = d.DocumentName,
                                   DocumentNumber = pl.DocumentNumber,
                                   FileUrl = pl.FileUrl,
                                   Note = pl.Note,
                                   ProjectDate = pl.ProjectDate,
                                   ProjectFlowId = pl.ProjectFlowId,
                                   PromulgateUnit = pl.PromulgateUnit,
                                   RegulationDocument = pl.RegulationDocument,
                                   Signer = pl.Signer,
                                   Status = pl.Status
                               }).ToListAsync();
                return Response<List<ProjectFlowGetListDto>>.CreateSuccessResponse(query);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<ProjectFlowGetListDto>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<ProjectFlow>> GetByIdAsync(Guid projectFlowId)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<ProjectFlow>().GetByExpression(n => n.ProjectFlowId == projectFlowId).FirstOrDefaultAsync();
                return Response<ProjectFlow>.CreateSuccessResponse(query);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<ProjectFlow>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<ProjectFlow>> UpdateAsync()
        {
            try
            {
                var projectFlow = JsonConvert.DeserializeObject<ProjectFlowCreateDto>(_httpContextAccessor.HttpContext.Request.Form["projectFlow"].ToString());
                var flow = await _unitOfWork.GetRepository<ProjectFlow>().GetByExpression(n => n.ProjectFlowId == projectFlow.ProjectFlowId).FirstOrDefaultAsync();
                if(flow == null)
                {
                    return Response<ProjectFlow>.CreateErrorResponse(new Exception("Không tìm thấy flow"));
                }
                if(_httpContextAccessor.HttpContext.Request.Form.Files?.Count > 0)
                {
                    var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];
                    string filePath = null;
                    if (file != null)
                    {
                        filePath = await UploadFile(file);
                        projectFlow.FileUrl = filePath;
                    }
                }
                projectFlow.FileUrl = (string.IsNullOrEmpty(projectFlow.FileUrl)) ? flow.FileUrl : projectFlow.FileUrl;
                var mapped = _mapper.Map<ProjectFlowCreateDto, ProjectFlow>(projectFlow, flow);
                await _unitOfWork.GetRepository<ProjectFlow>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<ProjectFlow>.CreateSuccessResponse(mapped);
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

        public async Task<Response<bool>> DeleteAsync(Guid projectFlowId)
        {
            try
            {
                var flow = await _unitOfWork.GetRepository<ProjectFlow>().GetByExpression(n => n.ProjectFlowId == projectFlowId).FirstOrDefaultAsync();
                var project = await _unitOfWork.GetRepository<Project>().GetByExpression(n => n.ProjectId == flow.ProjectId).FirstOrDefaultAsync();
                project.CurrentState = ProjectState.Excuting;
                await _unitOfWork.GetRepository<Project>().Update(project);
                await _unitOfWork.GetRepository<ProjectFlow>().DeleteByExpression(n => n.ProjectFlowId == projectFlowId);
                await _unitOfWork.SaveAsync();
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<List<ProjectFlowGetListDto>>> GetFlowSyntheticAsync(Guid projectId)
        {
            try
            {
                var joined = await (from pl in _unitOfWork.GetRepository<ProjectFlow>().GetAll()
                                    join d in _unitOfWork.GetRepository<Document>().GetAll() on pl.DocumentId equals d.DocumentId
                                    where pl.ProjectId == projectId && pl.IsMainDocument.Value
                                    orderby pl.ProjectDate ascending
                                    select new ProjectFlowGetListDto
                                    {
                                        BiddingPackageId = pl.BiddingPackageId,
                                        ProjectId = pl.ProjectId,
                                        DocumentId = pl.DocumentId,
                                        DocumentAbstract = pl.DocumentAbstract,
                                        DocumentName = d.DocumentName,
                                        DocumentNumber = pl.DocumentNumber,
                                        FileUrl = pl.FileUrl,
                                        Note = pl.Note,
                                        ProjectDate = pl.ProjectDate,
                                        ProjectFlowId = pl.ProjectFlowId,
                                        PromulgateUnit = pl.PromulgateUnit,
                                        RegulationDocument = pl.RegulationDocument,
                                        Signer = pl.Signer,
                                        Status = pl.Status,
                                        IsMainDocument = pl.IsMainDocument
                                    }).ToListAsync();
                return Response<List<ProjectFlowGetListDto>>.CreateSuccessResponse(joined);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<ProjectFlowGetListDto>>.CreateErrorResponse(ex);
            }
        }
    }
}
