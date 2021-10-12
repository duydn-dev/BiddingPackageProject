using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public Guid UserRoleId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
