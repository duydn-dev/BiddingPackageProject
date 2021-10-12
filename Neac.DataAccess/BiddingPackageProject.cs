using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("BiddingPackageProject")]
    public class BiddingPackageProject
    {
        [Key]
        public Guid BiddingPackageProjectId { get; set; }
        public Guid? BiddingPackageId { get; set; }
        public Guid? ProjectId { get; set; }

        public BiddingPackage BiddingPackage { get; set; }
        public Project Project { get; set; }
    }
}
