using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.MeetRooms
{
    public class MeetRoomStatusDto
    {
        public Guid MeetRoomId { get; set; }
        public string DomainUrl { get; set; }
        public string RoomName { get; set; }
        public int? MemberNumberInRoom { get; set; }
        public int? MemberOnline { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? IsOnline { get; set; }
    }
}
