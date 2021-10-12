using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }
        public string RoleName {get;set;}
        public string RoleCode { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? GroupRoleId { get; set; }

        public IList<UserRole> UserRoles { get; set; }
        public GroupRole GroupRole { get; set; }
    }
}
