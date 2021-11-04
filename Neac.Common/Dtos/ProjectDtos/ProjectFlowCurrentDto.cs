using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.ProjectDtos
{
    public class ProjectFlowCurrentDto
    {
        public Guid? BiddingPackageId { get; set; }
        public string BiddingPackageName { get; set; }
        public int? Order { get; set; }
        public int? DocumentCount { get; set; }
        public int? CurrentDocumentCount { get; set; }
    }
    public class ProjectFlowCreateDto
    {
        public Guid? ProjectFlowId { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? ProjectDate { get; set; }
        public string PromulgateUnit { get; set; }
        public string DocumentAbstract { get; set; }
        public string Signer { get; set; }
        public string RegulationDocument { get; set; }
        public string FileUrl { get; set; }
        public string Note { get; set; }
        public int? Status { get; set; }

        public Guid? BiddingPackageId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? DocumentId { get; set; }
    }
    public class ProjectFlowGetListDto
    {
        public Guid? ProjectFlowId { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? ProjectDate { get; set; }
        public string PromulgateUnit { get; set; }
        public string DocumentAbstract { get; set; }
        public string Signer { get; set; }
        public string RegulationDocument { get; set; }
        public string FileUrl { get; set; }
        public string Note { get; set; }
        public int? Status { get; set; }

        public Guid? BiddingPackageId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? DocumentId { get; set; }
        public string DocumentName { get; set; }
    }
}
