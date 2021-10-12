using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("GroupRole")]
    public class GroupRole
    {
        [Key]
        public Guid GroupRoleId { get; set; }
        public string GroupRoleName {get;set;}
        public string GroupRoleCode { get; set; }

        public List<Role> GroupRoles { get; set; }
    }
}
