using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.MeetRooms
{
    public class GetFilterMeetRoomDto
    {
        public string Domain { get; set; }
        public string RoomName { get; set; }
        public int? NumberMember { get; set; }
    }
}
