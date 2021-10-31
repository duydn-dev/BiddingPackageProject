using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("DocumentSetting")]
    public class DocumentSetting
    {
        [Key]
        public Guid DocumentSettingId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? BiddingPackageId { get; set; }
        public Guid? DocumentId { get; set; }
        public int? Order { get; set; }
    }
}
