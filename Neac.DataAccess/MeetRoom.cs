using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    [Table("MeetRoom")]
    public class MeetRoom
    {
        [Key]
        public Guid MeetRoomId { get; set; }
        public string DomainUrl { get; set; }
        public string RoomName { get; set; }
        public int? MemberNumberInRoom { get; set; }
        public int? MemberOnline { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? Status { get; set; }
    }
}
