using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("Document")]
    public class Document
    {
        [Key]
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string Note { get; set; }
        public bool? IsCommon { get; set; } // dùng chung cho nhiều gói thầu
        public Guid? BiddingPackageId { get; set; }

        public BiddingPackage BiddingPackage { get; set; }
    }
}
