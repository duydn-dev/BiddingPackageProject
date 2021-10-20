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
        public int? TotalDocument { get; set; }
        public int? CurrentNumberDocument { get; set; }
    }
}
