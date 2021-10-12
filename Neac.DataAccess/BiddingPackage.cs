using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("BiddingPackage")]
    public class BiddingPackage
    {
        [Key]
        public Guid BiddingPackageId { get; set; }
        public string BiddingPackageName { get; set; }
        public int? Order { get; set; }

        public List<BiddingPackageProject> BiddingPackageProjects { get; set; }
    }
}
