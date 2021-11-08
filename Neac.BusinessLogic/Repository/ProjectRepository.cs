using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common;
using Neac.Common.Const;
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
    public class ProjectRepository : IProjectRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProjectRepository(ILogRepository logRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
                var query = await _unitOfWork.GetRepository<Project>().GetAll()
                    .Include(n => n.BiddingPackageProjects)
                    .Where(n => n.ProjectId == projectId)
                    .FirstOrDefaultAsync();

                var biddingPackagesProjects = await (_unitOfWork.GetRepository<BiddingPackage>().GetAll().Join(
                        _unitOfWork.GetRepository<BiddingPackageProject>().GetAll().Where(n => n.ProjectId == query.ProjectId),
                        left => left.BiddingPackageId,
                        right => right.BiddingPackageId,
                        (l, r) => new BiddingPackageDto
                        {
                            BiddingPackageId = l.BiddingPackageId,
                            BiddingPackageName = l.BiddingPackageName,
                            Order = r.Order
                        })).ToListAsync();

                var mapped = _mapper.Map<Project, ProjectGetListDto>(query);
                mapped.BiddingPackageDtos = biddingPackagesProjects;
                return Response<ProjectGetListDto>.CreateSuccessResponse(mapped);
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
                request.ProjectDate = request.ProjectDate.Value.AddDays(1);
                request.ProjectId = Guid.NewGuid();
                request.CurrentState = 0;
                if (request?.BiddingPackageDtos.Count > 0)
                {
                    request?.BiddingPackageDtos.ForEach(async n =>
                    {
                        await _unitOfWork.GetRepository<BiddingPackageProject>().Add(new BiddingPackageProject
                        {
                            BiddingPackageProjectId = Guid.NewGuid(),
                            ProjectId = request.ProjectId,
                            BiddingPackageId = n.BiddingPackageId,
                            Order = n.Order
                        });
                    });
                }
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
                await _unitOfWork.GetRepository<BiddingPackageProject>().DeleteByExpression(n => n.ProjectId == projectId);
                await _unitOfWork.GetRepository<ProjectFlow>().DeleteByExpression(n => n.ProjectId == projectId);
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
        public async Task<Response<IEnumerable<ExportDataMainDto>>> GetExportDataAsync(Guid projectId)
        {
            try
            {
                string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                List<ExportDataDto> exportDataDtos = new List<ExportDataDto>();
                var mustImported = await (from bd in _unitOfWork.GetRepository<BiddingPackage>().GetAll()
                                          join bpp in _unitOfWork.GetRepository<BiddingPackageProject>().GetByExpression(n => n.ProjectId == projectId) on bd.BiddingPackageId equals bpp.BiddingPackageId
                                          join ds in _unitOfWork.GetRepository<DocumentSetting>().GetByExpression(n => n.ProjectId == projectId) on bpp.BiddingPackageId equals ds.BiddingPackageId into gr
                                          from grData in gr.DefaultIfEmpty()
                                          orderby grData.Order ascending
                                          select new
                                          {
                                              BiddingPackageId = bd.BiddingPackageId,
                                              BiddingPackageName = bd.BiddingPackageName,
                                              DocumentId = grData.DocumentId,
                                              Order = grData.Order
                                          }).ToListAsync();

                var imported = await (from pf in _unitOfWork.GetRepository<ProjectFlow>().GetAll()
                                      join d in _unitOfWork.GetRepository<Document>().GetAll() on pf.DocumentId equals d.DocumentId
                                      where pf.ProjectId == projectId
                                      select new
                                      {
                                          DocumentName = d.DocumentName,
                                          ProjectFlowId = pf.ProjectFlowId,
                                          DocumentNumber = pf.DocumentNumber,
                                          ProjectDate = pf.ProjectDate,
                                          PromulgateUnit = pf.PromulgateUnit,
                                          DocumentAbstract = pf.DocumentAbstract,
                                          Signer = pf.Signer,
                                          RegulationDocument = pf.RegulationDocument,
                                          FileUrl = pf.FileUrl,
                                          Note = pf.Note,
                                          Status = pf.Status,
                                          BiddingPackageId = pf.BiddingPackageId,
                                          ProjectId = pf.ProjectId,
                                          DocumentId = pf.DocumentId,
                                          IsMainDocument = pf.IsMainDocument
                                      }).ToListAsync();

                var result = (from m in mustImported
                              join i in imported on m.DocumentId equals i.DocumentId into gr
                              from grData in gr.DefaultIfEmpty()
                              select new
                              {
                                  BiddingPackageName = m.BiddingPackageName,
                                  DocumentName = grData?.DocumentName,
                                  DocumentNumber = grData?.DocumentNumber,
                                  ProjectDate = grData?.ProjectDate,
                                  PromulgateUnit = grData?.PromulgateUnit,
                                  DocumentAbstract = grData?.DocumentAbstract,
                                  Signer = grData?.Signer,
                                  RegulationDocument = grData?.RegulationDocument,
                                  FileUrl = grData?.FileUrl,
                                  Note = grData?.Note,
                                  Status = grData?.Status,
                                  DocumentId = grData?.DocumentId,
                                  IsMainDocument = grData?.IsMainDocument
                              }).GroupBy(n => n.BiddingPackageName, (key, val) => new ExportDataMainDto
                              {
                                  BiddingPackageName = key,
                                  Documents = val.Where(g => g.DocumentId != null).Select((g, i) => new ProjectFlowExportCommonDto
                                  {
                                      Index = i + 1,
                                      DocumentName = g.DocumentName,
                                      DocumentNumber = g.DocumentNumber,
                                      ProjectDate = g.ProjectDate.Value.ToString("dd/MM/yyyy"),
                                      PromulgateUnit = g.PromulgateUnit,
                                      DocumentAbstract = g.DocumentAbstract,
                                      Signer = g.Signer,
                                      RegulationDocument = g.RegulationDocument,
                                      FileUrl = host + "/" + g.FileUrl.Replace("\\", "/"),
                                      Note = g.Note,
                                      Status = CommonFunction.DocumentStateName(g.Status),
                                      IsMainDocument = g.IsMainDocument
                                  }).ToArray()
                              });

                return Response<IEnumerable<ExportDataMainDto>>.CreateSuccessResponse(result);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<IEnumerable<ExportDataMainDto>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<ProjectGetStatisticalDto>> GetProjectStatisticalAsync()
        {
            try
            {
                var query = _unitOfWork.GetRepository<Project>().GetAll();
                var response = new ProjectGetStatisticalDto();
                response.TotalProject = query.Count();
                response.NumberProjectComplete = query.Count(n => n.CurrentState == ProjectState.Excuted);
                response.NumberProjectNotComplete = query.Count(n => n.CurrentState == ProjectState.Excuting);
                response.RatioProjectComplete = CommonFunction.ValidRatio(Convert.ToDouble(response.NumberProjectComplete), Convert.ToDouble(response.TotalProject));
                response.RatioProjectNotComplete = CommonFunction.ValidRatio(Convert.ToDouble(response.NumberProjectNotComplete), Convert.ToDouble(response.TotalProject));
                return Response<ProjectGetStatisticalDto>.CreateSuccessResponse(response);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<ProjectGetStatisticalDto>.CreateErrorResponse(ex);
            }
        }
    }
}
