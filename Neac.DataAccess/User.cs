using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }
        public Guid? UserPositionId { get; set; }


        public IList<UserRole> UserRoles { get; set; }
        public UserPosition UserPosition { get; set; }
        public User()
        {
            UserRoles = new List<UserRole>();
        }
    }
}
