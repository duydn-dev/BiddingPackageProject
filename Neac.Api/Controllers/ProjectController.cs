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
        [HttpPost]
        [RoleDescription("Xem danh sách dự án")]
        public async Task<Response<GetListResponseModel<List<ProjectGetListDto>>>> GetFilterAsync([FromQuery] string filter)
        {
            return await _projectRepository.GetFilter(filter);
        }
    }
}
