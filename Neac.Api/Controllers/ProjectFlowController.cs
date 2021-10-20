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
        public async Task<Response<GetListResponseModel<List<ProjectFlow>>>> GetFilterAsync(string filter)
        {
            return await _projectFlowRepository.GetFilterAsync(filter);
        }
        [Route("current-state/{projectId}")]
        [HttpGet]
        //[RoleDescription("Tình trạng hiện tại dự án")]
        [AllowAnonymous]
        public async Task<Response<List<ProjectFlowCurrentDto>>> CurrentState(Guid projectId)
        {
            return await _projectFlowRepository.CurrentState(projectId);
        }

        [Route("create")]
        [HttpPost]
        [RoleDescription("Thêm mới luồng dự án")]
        public async Task<Response<ProjectFlow>> CreateAsync(ProjectFlow request)
        {
            return await _projectFlowRepository.CreateAsync(request);
        }

        [Route("update/{projectFlowId}")]
        [HttpPut]
        [RoleDescription("Chỉnh sửa luồng dự án")]
        public async Task<Response<ProjectFlow>> UpdateAsync([FromRoute] Guid projectFlowId, [FromBody] ProjectFlow request)
        {
            request.ProjectFlowId = projectFlowId;
            return await _projectFlowRepository.UpdateAsync(request);
        }
    }
}
