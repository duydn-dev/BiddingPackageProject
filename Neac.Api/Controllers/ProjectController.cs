using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.Common.Dtos.ProjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý dự án")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [Route("")]
        [HttpGet]
        [RoleDescription("Xem danh sách dự án")]
        public async Task<Response<GetListResponseModel<List<ProjectGetListDto>>>> GetFilterAsync([FromQuery] string filter)
        {
            return await _projectRepository.GetFilter(filter);
        }

        [Route("{projectId}")]
        [HttpGet]
        [RoleDescription("Xem chi tiết dự án")]
        public async Task<Response<ProjectGetListDto>> GetByIdAsync(Guid projectId)
        {
            return await _projectRepository.GetByIdAsync(projectId);
        }

        [Route("create")]
        [HttpPost]
        [RoleDescription("Thêm mới dự án")]
        public async Task<Response<ProjectGetListDto>> CreateAsync(ProjectGetListDto request)
        {
            return await _projectRepository.CreateAsync(request);
        }
        [Route("update/{projectId}")]
        [HttpPut]
        [RoleDescription("Cập nhật dự án")]
        public async Task<Response<ProjectGetListDto>> UpdateAsync([FromRoute]Guid projectId, ProjectGetListDto request)
        {
            request.ProjectId = projectId;
            return await _projectRepository.UpdateAsync(request);
        }
        [Route("delete/{projectId}")]
        [HttpDelete]
        [RoleDescription("Xóa dự án")]
        public async Task<Response<bool>> DeleteAsync(Guid projectId)
        {
            return await _projectRepository.DeleteAsync(projectId);
        }
    }
}
