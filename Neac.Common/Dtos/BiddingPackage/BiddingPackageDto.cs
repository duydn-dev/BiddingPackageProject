using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.BiddingPackage
{
    public class BiddingPackageDto
    {
        public Guid BiddingPackageId { get; set; }
        public string BiddingPackageName { get; set; }
        public int? Order { get; set; }
    }
}
