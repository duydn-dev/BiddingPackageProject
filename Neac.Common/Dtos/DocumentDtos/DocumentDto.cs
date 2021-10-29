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
    public class DocumentByPackageIdDto
    {
        public Guid? PackageId { get; set; }
        public Guid? ProjectId { get; set; }
    }
    public class PackageListByProjectDto
    {
        public Guid? BiddingPackageId { get; set; }
        public string BiddingPackageName { get; set; }
        public IEnumerable<DocumentListByProjectDto> Documents { get; set; }
    }
    public class DocumentListByProjectDto
    {
        public Guid? DocumentId {get; set;} 
        public string DocumentName {get; set;}
        public bool? IsCommon {get; set;}
        public int? Order { get; set; }
    }
}
