using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.ProjectDtos
{
    public class ListByPackageExportDto
    {
        public string BiddingPackageName { get; set; }
        public ProjectFlow Document { get; set; }
    }
    public class ListByPackageMustImportedDto
    {
        public string BiddingPackageName { get; set; }
        public DocumentSetting Document { get; set; }
    }
    public class ProjectFlowExportDto
    {
        public int Index { get; set; }
        public string DocumentName { get; set; }
        public string DocumentNumber { get; set; }
        public string ProjectDate { get; set; }
        public string PromulgateUnit { get; set; }
        public string DocumentAbstract { get; set; }
        public string Signer { get; set; }
        public string RegulationDocument { get; set; }
        public string FileUrl { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
    }
    public class ProjectFlowExportCommonDto
    {
        public int Index { get; set; }
        public string DocumentName { get; set; }
        public string DocumentNumber { get; set; }
        public string ProjectDate { get; set; }
        public string PromulgateUnit { get; set; }
        public string DocumentAbstract { get; set; }
        public string Signer { get; set; }
        public string RegulationDocument { get; set; }
        public string FileUrl { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public bool? IsMainDocument { get; set; }
    }
    public class ExportDataDto
    {
        public string BiddingPackageName { get; set; }
        public ProjectFlowExportDto[] Documents { get; set; }
    }
    public class ExportDataMainDto
    {
        public string BiddingPackageName { get; set; }
        public ProjectFlowExportCommonDto[] Documents { get; set; }
    }
    public class ProjectGetStatisticalDto
    {
        public int? TotalProject { get; set; }
        public int? NumberProjectNotComplete { get; set; }
        public int? NumberProjectComplete { get; set; }
        public double? RatioProjectNotComplete { get; set; }
        public double? RatioProjectComplete { get; set; }
    }
}
