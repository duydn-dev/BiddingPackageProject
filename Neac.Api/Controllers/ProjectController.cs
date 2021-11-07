using Aspose.Cells;
using FastMember;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Const;
using Neac.Common.Dtos;
using Neac.Common.Dtos.ProjectDtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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
        [Route("get-project-statistical")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<Response<ProjectGetStatisticalDto>> GetProjectStatisticalAsync()
        {
            return await _projectRepository.GetProjectStatisticalAsync();
        }

        [Route("export-excel/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        [Obsolete]
        public async Task<ActionResult> ExportExcelAsync(Guid projectId)
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
                        var response = await _projectRepository.GetExportDataAsync(projectId);
                        var responseData = response.ResponseData.ToArray();

                        // draw template
                        for (int i = 0; i < responseData.Count(); i++)
                        {
                            Worksheet worksheet = (i > 0) ? workbook.Worksheets[workbook.Worksheets.Add()]: workbook.Worksheets[0];
                            worksheet.Name = responseData[i].BiddingPackageName;
                            
                            DataTable data = responseData[i].Documents.RenameHeaderAndConvertToDatatable(new List<string> {
                                "STT",
                                "Tên văn bản",
                                "Số hiệu",
                                "Ngày ký",
                                "Đơn vị cung cấp",
                                "Tóm tắt",
                                "Người ký",
                                "Văn bản quy định",
                                "Đường dẫn File",
                                "Ghi chú",
                                "Tình trạng"
                            });
                            worksheet.Cells.ImportDataTable(data, true, "A1");
                            worksheet.AutoFitColumns();

                            Aspose.Cells.Range range = worksheet.Cells.CreateRange(0, 0, worksheet.Cells.Rows.Count, worksheet.Cells.Columns.Count);
                            Style style = workbook.CreateStyle();
                            style.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
                            style.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);
                            style.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
                            style.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
                            style.Font.Name = "Times New Roman";
                            range.SetStyle(style);
                            if (worksheet.Cells.Rows.Count > 0)
                            {
                                Aspose.Cells.Range range2 = worksheet.Cells.CreateRange(0, 0, 1, worksheet.Cells.Columns.Count);
                                Style headerStyle = workbook.CreateStyle();
                                headerStyle.Font.IsBold = true;
                                range2.SetStyle(headerStyle);
                            }
                        }
                        MemoryStream memoryStream = new MemoryStream();
                        workbook.Save(memoryStream, SaveFormat.Xlsx);
                        var by = memoryStream.ToArray();
                        await memoryStream.DisposeAsync();
                        memoryStream.Close();
                        return File(by, "application/octet-stream", "export-data.xlsx");
                    }
                    return NotFound();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return NotFound();
                }
            }
            return NotFound();
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
