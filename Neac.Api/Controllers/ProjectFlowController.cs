using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.Common.Dtos.ProjectDtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý luồng dự án")]
    public class ProjectFlowController : ControllerBase
    {
        private readonly IProjectFlowRepository _projectFlowRepository;
        public ProjectFlowController(IProjectFlowRepository projectFlowRepository)
        {
            _projectFlowRepository = projectFlowRepository;
        }

        [Route("")]
        [HttpGet]
        [RoleDescription("Xem danh sách luồng dự án")]
        public async Task<Response<List<ProjectFlow>>> GetFilterAsync([FromQuery]string filter)
        {
            return await _projectFlowRepository.GetFilterAsync(filter);
        }
        [Route("current-state/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<Response<List<ProjectFlowCurrentDto>>> CurrentStateAsync(Guid projectId)
        {
            return await _projectFlowRepository.CurrentState(projectId);
        }
        [Route("current-package/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<Response<Guid>> CurrentPackageAsync(Guid projectId)
        {
            return await _projectFlowRepository.CurrentPackageAsync(projectId);
        }

        [Route("create")]
        [HttpPost]
        [RoleDescription("Thêm mới luồng dự án")]
        public async Task<Response<ProjectFlow>> CreateAsync()
        {
            return await _projectFlowRepository.CreateAsync();
        }

        [Route("update/{projectFlowId}")]
        [HttpPut]
        [RoleDescription("Chỉnh sửa luồng dự án")]
        public async Task<Response<ProjectFlow>> UpdateAsync([FromRoute] Guid projectFlowId, [FromBody] ProjectFlow request)
        {
            request.ProjectFlowId = projectFlowId;
            return await _projectFlowRepository.UpdateAsync(request);
        }

        [Route("download/{projectFlowId}")]
        [HttpGet]
        [RoleDescription("Tải xuống văn bản")]
        public async Task<IActionResult> DownloadAsync([FromRoute] Guid projectFlowId)
        {
            var filePath = await _projectFlowRepository.DownloadAsync(projectFlowId);
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}
