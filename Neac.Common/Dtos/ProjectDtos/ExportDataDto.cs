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
    public class ExportDataDto
    {
        public string BiddingPackageName { get; set; }
        public List<ProjectFlowExportDto> Documents { get; set; }
    }
}
