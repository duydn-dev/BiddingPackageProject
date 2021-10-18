using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.DocumentDtos
{
    public class DocumentDto
    {
        public Guid? DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string Note { get; set; }
        public bool? IsCommon { get; set; } // dùng chung cho nhiều gói thầu
        public Guid? BiddingPackageId { get; set; }
    }
}
