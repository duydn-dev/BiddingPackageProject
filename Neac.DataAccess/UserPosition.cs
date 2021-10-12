using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("UserPosition")]
    public class UserPosition
    {
        [Key]
        public Guid UserPositionId { get; set; }
        public string UserPositionName { get; set; }
        public bool? IsAdministrator { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }

        public List<User> Users { get; set; }
    }
}
