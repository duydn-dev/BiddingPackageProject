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
        public async Task<Response<List<ProjectFlowGetListDto>>> GetFilterAsync([FromQuery]string filter)
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

        [Route("get-by-id/{projectFlowId}")]
        [HttpGet]
        [RoleDescription("Xem quy trình thực thi dự án")]
        public async Task<Response<ProjectFlow>> GetByIdAsync([FromRoute] Guid projectFlowId)
        {
            return await _projectFlowRepository.GetByIdAsync(projectFlowId);
        }

        [Route("create")]
        [HttpPost]
        [RoleDescription("Thêm mới luồng dự án")]
        public async Task<Response<ProjectFlow>> CreateAsync()
        {
            return await _projectFlowRepository.CreateAsync();
        }

        [Route("update")]
        [HttpPost]
        [RoleDescription("Chỉnh sửa luồng dự án")]
        public async Task<Response<ProjectFlow>> UpdateAsync()
        {
            return await _projectFlowRepository.UpdateAsync();
        }
        [Route("delete/{projectFlowId}")]
        [HttpDelete]
        [RoleDescription("Xóa văn bản trong luồng dự án")]
        public async Task<Response<bool>> DeleteAsync([FromRoute] Guid projectFlowId)
        {
            return await _projectFlowRepository.DeleteAsync(projectFlowId);
        }

        [Route("download/{projectFlowId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadAsync([FromRoute] Guid projectFlowId)
        {
            var filePath = await _projectFlowRepository.DownloadAsync(projectFlowId);
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}
