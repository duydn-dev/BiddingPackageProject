using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.BiddingPackage
{
    public class BiddingPackageProjectDto
    {
        public Guid? BiddingPackageProjectId { get; set; }
        public Guid? BiddingPackageId { get; set; }
        public Guid? ProjectId { get; set; }
        public int? Order { get; set; }
    }
}
