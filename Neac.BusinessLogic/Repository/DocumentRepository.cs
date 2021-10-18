using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common;
using Neac.Common.Dtos;
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
        public DocumentRepository(ILogRepository logRepository, IUnitOfWork unitOfWork)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
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
        public async Task<Response<Document>> CreateAsync(Document request)
        {
            try
            {
                request.DocumentId = Guid.NewGuid();
                await _unitOfWork.GetRepository<Document>().Add(request);
                await _unitOfWork.SaveAsync();
                return Response<Document>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<Document>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<Document>> UpdateAsync(Document request)
        {
            try
            {
                var project = await _unitOfWork.GetRepository<Document>().GetByExpression(n => n.DocumentId == request.DocumentId).FirstOrDefaultAsync();
                await _unitOfWork.GetRepository<Document>().Update(project);
                await _unitOfWork.SaveAsync();
                return Response<Document>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<Document>.CreateErrorResponse(ex);
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
    }
}
