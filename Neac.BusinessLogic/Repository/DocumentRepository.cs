using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common;
using Neac.Common.Dtos;
using Neac.Common.Dtos.DocumentDtos;
using Neac.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DocumentRepository(ILogRepository logRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<GetListResponseModel<List<Document>>>> GetFilter(string filter)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<GetListUserRequestDto>(filter);
                var query = _unitOfWork.GetRepository<Document>()
                                    .GetAll()
                                    .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.DocumentName.Contains(request.TextSearch) || n.Note.Contains(request.TextSearch));

                GetListResponseModel<List<Document>> responseData = new GetListResponseModel<List<Document>>(query.Count(), request.PageSize);
                var result = await query
                    .OrderByDescending(n => n.DocumentName)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();

                responseData.Data = result;
                return Response<GetListResponseModel<List<Document>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<Document>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<List<Document>>> GetDropdownAsync()
        {
            try
            {
                var query = await _unitOfWork.GetRepository<Document>().GetAll()
                    .ToListAsync();

                return Response<List<Document>>.CreateSuccessResponse(query);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<Document>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<Document>> GetByIdAsync(Guid documentId)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<Document>().GetAll()
                    .Where(n => n.DocumentId == documentId)
                    .FirstOrDefaultAsync();

                return Response<Document>.CreateSuccessResponse(query);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<Document>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<DocumentDto>> CreateAsync(DocumentDto request)
        {
            try
            {
                request.DocumentId = Guid.NewGuid();
                request.IsCommon = request.IsCommon ?? false;
                var mapped = _mapper.Map<DocumentDto, Document>(request);
                await _unitOfWork.GetRepository<Document>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<DocumentDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<DocumentDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<DocumentDto>> UpdateAsync(DocumentDto request)
        {
            try
            {
                var project = await _unitOfWork.GetRepository<Document>().GetByExpression(n => n.DocumentId == request.DocumentId).FirstOrDefaultAsync();
                var mapped = _mapper.Map<DocumentDto, Document>(request, project);
                await _unitOfWork.GetRepository<Document>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<DocumentDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<DocumentDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> DeleteAsync(Guid documentId)
        {
            try
            {
                await _unitOfWork.GetRepository<Document>().DeleteByExpression(n => n.DocumentId == documentId);
                await _unitOfWork.SaveAsync();
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<List<Document>>> GetDropdownByPackageIdAsync(DocumentByPackageIdDto request)
        {
            try
            {
                 //lấy ra những thằng đã nhập
                var listImported = await _unitOfWork.GetRepository<ProjectFlow>()
                    .GetByExpression(n => n.ProjectId == request.ProjectId && n.BiddingPackageId == request.PackageId)
                    .Select(n => n.DocumentId)
                    .ToListAsync();

                //lấy ra những thằng phải nhập
                var mustImport = await _unitOfWork.GetRepository<DocumentSetting>()
                    .GetByExpression(n => n.BiddingPackageId == request.PackageId && n.ProjectId == request.ProjectId)
                    .Select(n => n.DocumentId)
                    .ToListAsync();

                // lấy ra những thằng chưa nhập
                var notImport = mustImport.Except(listImported).Union(listImported.Except(mustImport));

                var document = await (from d in _unitOfWork.GetRepository<Document>().GetAll()
                                      join ds in _unitOfWork.GetRepository<DocumentSetting>().GetAll() on d.DocumentId equals ds.DocumentId
                                      where ds.ProjectId == request.ProjectId && ds.BiddingPackageId == request.PackageId
                                      select new Document
                                      {
                                          BiddingPackageId = d.BiddingPackageId,
                                          DocumentId = d.DocumentId,
                                          DocumentName = $"{ds.Order}. {d.DocumentName}",
                                          IsCommon = d.IsCommon,
                                          Note = d.Note
                                      }).ToListAsync();
                return Response<List<Document>>.CreateSuccessResponse(document, listImported);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<Document>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<IEnumerable<PackageListByProjectDto>>> GetSettingDocumentAsync(Guid projectId)
        {
            try
            {
                // lấy ra danh sách văn bản thuộc gói thầu
                var query = await (from b in _unitOfWork.GetRepository<BiddingPackage>().GetAll()
                        join bp in _unitOfWork.GetRepository<BiddingPackageProject>().GetAll() on b.BiddingPackageId equals bp.BiddingPackageId
                        join d in _unitOfWork.GetRepository<Document>().GetAll() on b.BiddingPackageId equals d.BiddingPackageId
                        where bp.ProjectId == projectId
                        select new
                        {
                            b.BiddingPackageId,
                            b.BiddingPackageName,
                            d.DocumentId,
                            d.DocumentName,
                            d.IsCommon
                        }).ToListAsync();
                var queryData = query.GroupBy(b => new
                {
                    b.BiddingPackageId,
                    b.BiddingPackageName,
                }, (key, data) => new PackageListByProjectDto
                {
                    BiddingPackageId = key.BiddingPackageId,
                    BiddingPackageName = key.BiddingPackageName,
                    Documents = data.Select(n => new DocumentListByProjectDto 
                    { 
                        DocumentId = n.DocumentId, 
                        DocumentName = n.DocumentName, 
                        IsCommon = n.IsCommon
                    })
                });

                // lấy ra dữ liệu danh sách văn bản thuộc gói thầu đc cấu hình
                var documentSettings = await _unitOfWork.GetRepository<DocumentSetting>()
                    .GetByExpression(n => n.ProjectId == projectId)
                    .OrderBy(n => n.Order)
                    .ToListAsync();
                return Response<IEnumerable<PackageListByProjectDto>>.CreateSuccessResponse(queryData, documentSettings);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<IEnumerable<PackageListByProjectDto>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<List<DocumentSetting>>> GetSettingsSelectedAsync(Guid projectId)
        {
            try
            {
                var settings = await _unitOfWork.GetRepository<DocumentSetting>().GetByExpression(n => n.ProjectId == projectId).ToListAsync();
                return Response<List<DocumentSetting>>.CreateSuccessResponse(settings);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<DocumentSetting>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<Guid>> SaveDocumentSettingAsync(Guid projectId, List<DocumentSettingCreateDto> request)
        {
            try
            {
                if(request.Count > 0)
                {
                    var settings = await _unitOfWork.GetRepository<DocumentSetting>().DeleteByExpression(n => n.ProjectId == projectId);
                    foreach (var packageItem in request)
                    {
                        if (packageItem.Documents?.Count > 0)
                        {
                            foreach (var item in packageItem.Documents)
                            {
                                await _unitOfWork.GetRepository<DocumentSetting>().Add(
                                    new DocumentSetting
                                    {
                                        BiddingPackageId = packageItem.BiddingPackageId,
                                        DocumentId = item.DocumentId,
                                        DocumentSettingId = Guid.NewGuid(),
                                        Order = item.Order,
                                        ProjectId = projectId
                                    });
                            }
                        }
                    }
                    await _unitOfWork.SaveAsync();
                    return Response<Guid>.CreateSuccessResponse(projectId);
                }
              return Response<Guid>.CreateErrorResponse(new Exception("Danh sách cấu hình văn bản bị trống !"));
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<Guid>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<List<Document>>> GetSyntheticDocumentAsync()
        {
            try
            {
                var data = await _unitOfWork.GetRepository<Document>().GetByExpression(n => n.BiddingPackageId == null).ToListAsync();
                return Response<List<Document>>.CreateSuccessResponse(data);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<Document>>.CreateErrorResponse(ex);
            }
        }
    }
}
