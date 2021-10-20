using Neac.Common.Dtos.DocumentDtos;
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
    public class BiddingPackageByIdDto
    {
        public Guid? BiddingPackageId { get; set; }
        public string BiddingPackageName { get; set; }
        public List<DocumentDto> Documents { get; set; }
    }
    public class BiddingPackageTotalDocumentDto
    {
        public Guid? BiddingPackageId { get; set; }
        public int? NumberDocument { get; set; }
    }
}
