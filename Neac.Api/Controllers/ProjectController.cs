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
        private readonly IProjectFlowRepository _projectFlowRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogRepository _logRepository;
        public ProjectController(IProjectRepository projectRepository, IWebHostEnvironment webHostEnvironment, ILogRepository logRepository, IProjectFlowRepository projectFlowRepository)
        {
            _projectRepository = projectRepository;
            _webHostEnvironment = webHostEnvironment;
            _logRepository = logRepository;
            _projectFlowRepository = projectFlowRepository;
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
                        List<string> header = new List<string> {
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
                            };

                        // tổng hợp
                        var synthetic = await _projectFlowRepository.GetFlowSyntheticAsync(projectId);
                        var listSynthetic = synthetic.ResponseData.Select((g, i) => new ProjectFlowExportDto 
                        {
                            DocumentAbstract = g.DocumentAbstract,
                            DocumentName = g.DocumentName,
                            DocumentNumber = g.DocumentNumber,
                            FileUrl = g.FileUrl,
                            Index = (i + 1),
                            Note = g.Note,
                            ProjectDate = g.ProjectDate.Value.ToString("dd/MM/yyyy"),
                            PromulgateUnit = g.PromulgateUnit,
                            RegulationDocument = g.RegulationDocument,
                            Signer = g.Signer,
                            Status = CommonFunction.DocumentStateName(g.Status),
                        }).RenameHeaderAndConvertToDatatable(header);

                        var response = await _projectRepository.GetExportDataAsync(projectId);
                        var responseData = response.ResponseData.ToArray();

                        // văn bản theo gói thầu
                        var child = responseData.Select(n => new ExportDataDto
                        {
                            BiddingPackageName = n.BiddingPackageName,
                            Documents = n?.Documents?.Length > 0 ? n.Documents.Select(g => new ProjectFlowExportDto
                            {
                                DocumentAbstract = g.DocumentAbstract,
                                DocumentName = g.DocumentName,
                                DocumentNumber = g.DocumentNumber,
                                FileUrl = g.FileUrl,
                                Index = g.Index,
                                Note = g.Note,
                                ProjectDate = g.ProjectDate,
                                PromulgateUnit = g.PromulgateUnit,
                                RegulationDocument = g.RegulationDocument,
                                Signer = g.Signer,
                                Status = g.Status
                            }).ToArray() : new ProjectFlowExportDto[0]
                        }).ToArray();

                        Worksheet worksheet;
                        worksheet = workbook.Worksheets[0];
                        worksheet.Name = "Tổng hợp văn bản chính";

                        worksheet.Cells.ImportData(listSynthetic, 0, 0, new ImportTableOptions() { });
                        worksheet.AutoFitColumns();

                        Aspose.Cells.Range ranges1 = worksheet.Cells.CreateRange(0, 0, worksheet.Cells.Rows.Count, worksheet.Cells.Columns.Count);
                        Style styles1 = workbook.CreateStyle();
                        styles1.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
                        styles1.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);
                        styles1.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
                        styles1.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
                        styles1.Font.Name = "Times New Roman";
                        ranges1.SetStyle(styles1);
                        if (worksheet.Cells.Rows.Count > 0)
                        {
                            Aspose.Cells.Range range2 = worksheet.Cells.CreateRange(0, 0, 1, worksheet.Cells.Columns.Count);
                            Style headerStyle = workbook.CreateStyle();
                            headerStyle.Font.IsBold = true;
                            range2.SetStyle(headerStyle);
                        }


                        // draw template
                        for (int i = 1; i <= child.Count(); i++)
                        {
                            worksheet = workbook.Worksheets[workbook.Worksheets.Add()];
                            worksheet = workbook.Worksheets[i];
                            worksheet.Name = child[i - 1].BiddingPackageName;

                            DataTable data = child[i - 1].Documents.RenameHeaderAndConvertToDatatable(header);
                            worksheet.Cells.ImportData(data, 0, 0, new ImportTableOptions() { });
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
                    return Content("workbook.IsLicensed = false");
                }
                catch (Exception ex)
                {
                    await _logRepository.ErrorAsync(ex);
                    return Content(ex.ToString());
                }
            }
            return Content("License file not found");
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
        public async Task<Response<ProjectGetListDto>> UpdateAsync([FromRoute] Guid projectId, ProjectGetListDto request)
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
