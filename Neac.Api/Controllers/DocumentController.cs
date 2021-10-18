using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
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
        public async Task<Response<Document>> CreateAsync(Document request)
        {
            return await _documentRepository.CreateAsync(request);
        }
        [Route("update/{projectId}")]
        [HttpPut]
        [RoleDescription("Cập nhật văn bản")]
        public async Task<Response<Document>> UpdateAsync([FromRoute] Guid projectId, Document request)
        {
            request.DocumentId = projectId;
            return await _documentRepository.UpdateAsync(request);
        }
        [Route("delete/{projectId}")]
        [HttpDelete]
        [RoleDescription("Xóa văn bản")]
        public async Task<Response<bool>> DeleteAsync(Guid projectId)
        {
            return await _documentRepository.DeleteAsync(projectId);
        }
    }
}
