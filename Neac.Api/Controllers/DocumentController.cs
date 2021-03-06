using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.Common.Dtos.DocumentDtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý văn bản")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        public DocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        [Route("")]
        [HttpGet]
        [RoleDescription("Xem danh sách văn bản")]
        public async Task<Response<GetListResponseModel<List<Document>>>> GetFilterAsync([FromQuery] string filter)
        {
            return await _documentRepository.GetFilter(filter);
        }

        [AllowAnonymous]
        [Route("dropdown")]
        [HttpGet]
        public async Task<Response<List<Document>>> GetDropdownAsync()
        {
            return await _documentRepository.GetDropdownAsync();
        }
        [AllowAnonymous]
        [Route("dropdown-by-packageid")]
        [HttpPost]
        public async Task<Response<List<Document>>> GetDropdownByPackageIdAsync([FromBody] DocumentByPackageIdDto request)
        {
            return await _documentRepository.GetDropdownByPackageIdAsync(request);
        }
        [AllowAnonymous]
        [Route("get-setting-document/{projectId}")]
        [HttpGet]
        public async Task<Response<IEnumerable<PackageListByProjectDto>>> GetSettingDocumentAsync([FromRoute] Guid projectId)
        {
            return await _documentRepository.GetSettingDocumentAsync(projectId);
        }

        [RoleDescription("Lấy danh sách chọn văn bản tổng hợp")]
        [Route("get-synthetic-document")]
        [HttpGet]
        public async Task<Response<List<Document>>> GetSyntheticDocumentAsync()
        {
            return await _documentRepository.GetSyntheticDocumentAsync();
        }
        
        [AllowAnonymous]
        [Route("get-setting-selected/{projectId}")]
        [HttpGet]
        public async Task<Response<List<DocumentSetting>>> GetSettingsSelectedAsync(Guid projectId)
        {
            return await _documentRepository.GetSettingsSelectedAsync(projectId);
        }

        [Route("{projectId}")]
        [HttpGet]
        [RoleDescription("Xem chi tiết văn bản")]
        public async Task<Response<Document>> GetByIdAsync(Guid projectId)
        {
            return await _documentRepository.GetByIdAsync(projectId);
        }

        [Route("create")]
        [HttpPost]
        [RoleDescription("Thêm mới văn bản")]
        public async Task<Response<DocumentDto>> CreateAsync(DocumentDto request)
        {
            return await _documentRepository.CreateAsync(request);
        }
        [Route("update/{documentId}")]
        [HttpPut]
        [RoleDescription("Cập nhật văn bản")]
        public async Task<Response<DocumentDto>> UpdateAsync([FromRoute] Guid documentId, DocumentDto request)
        {
            request.DocumentId = documentId;
            return await _documentRepository.UpdateAsync(request);
        }
        [Route("delete/{projectId}")]
        [HttpDelete]
        [RoleDescription("Xóa văn bản")]
        public async Task<Response<bool>> DeleteAsync(Guid projectId)
        {
            return await _documentRepository.DeleteAsync(projectId);
        }
        [Route("save-document-setting/{projectId}")]
        [HttpPut]
        [RoleDescription("Lưu cấu hình văn bản")]
        public async Task<Response<Guid>> SaveDocumentSettingAsync([FromRoute]Guid projectId,  [FromBody] List<DocumentSettingCreateDto> request)
        {
            return await _documentRepository.SaveDocumentSettingAsync(projectId, request);
        }
    }
}
