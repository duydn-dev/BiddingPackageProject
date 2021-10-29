using Aspose.Cells;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý dự án")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProjectController(IProjectRepository projectRepository, IWebHostEnvironment webHostEnvironment)
        {
            _projectRepository = projectRepository;
            _webHostEnvironment = webHostEnvironment;
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
        [Route("export-excel")]
        [HttpGet]
        //[RoleDescription("Xuất file excel")]
        [AllowAnonymous]
        public async Task<JsonResult> ExportExcelAsync()
        {
            var licensePath = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, "Libs", "License.lic");
            if (System.IO.File.Exists(licensePath))
            {
                Workbook workbook = new Workbook();
                try
                {
                    License lic = new License();
                    lic.SetLicense(licensePath);
                    if (workbook.IsLicensed)
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return new JsonResult(new { });
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
