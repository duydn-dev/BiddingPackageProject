using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("Project")]
    public class Project
    {
        [Key]
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime? ProjectDate { get; set; }
        public string Note { get; set; }
        public int? CurrentState { get; set; }

        public List<BiddingPackageProject> BiddingPackageProjects { get; set; }
    }
}
