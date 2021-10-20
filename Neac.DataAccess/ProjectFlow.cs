using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("ProjectFlow")]
    public class ProjectFlow
    {
        [Key]
        public Guid ProjectFlowId { get; set; }
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
}
