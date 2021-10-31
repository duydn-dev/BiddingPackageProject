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
                var listImported = await _unitOfWork.GetRepository<ProjectFlow>()
                    .GetByExpression(n => n.ProjectId == request.ProjectId && n.BiddingPackageId == request.PackageId)
                    .Select(n => n.DocumentId)
                    .ToListAsync();
                //var documents = await _unitOfWork.GetRepository<Document>()
                //    .GetByExpression(n => (n.IsCommon.Value && n.BiddingPackageId == null) || n.BiddingPackageId == request.PackageId)
                //    .WhereIf(listImported.Count > 0, n => !listImported.Contains(n.DocumentId))
                //    .ToListAsync();
                var document = await _unitOfWork.GetRepository<Document>()
                    .GetByExpression(n => (n.IsCommon.Value && n.BiddingPackageId == null) || n.BiddingPackageId == request.PackageId)
                    //.WhereIf(listImported.Count > 0, n => !listImported.Contains(n.DocumentId))
                    .ToListAsync();
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

        public async Task<Response<Guid>> SaveDocumentSettingAsync(Guid projectId, DocumentSettingCreateDto request)
        {
            try
            {
                var settings = await _unitOfWork.GetRepository<DocumentSetting>().DeleteByExpression(n => n.ProjectId == projectId);
                if(request.Documents?.Count > 0)
                {
                    foreach (var item in request.Documents)
                    {
                        await _unitOfWork.GetRepository<DocumentSetting>().Add(
                            new DocumentSetting
                            {
                                BiddingPackageId = request.BiddingPackageId,
                                DocumentId = item.DocumentId,
                                DocumentSettingId = Guid.NewGuid(),
                                Order = item.Order,
                                ProjectId = projectId
                            });
                    }
                }
                await _unitOfWork.SaveAsync();
                return Response<Guid>.CreateSuccessResponse(projectId);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<Guid>.CreateErrorResponse(ex);
            }
        }
    }
}
