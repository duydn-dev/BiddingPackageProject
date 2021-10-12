using Neac.Common.Dtos;
using Neac.Common.Dtos.MeetRooms;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IMeetRoomRepository
    {
        Task<Response<List<MeetRoom>>> GetMeetRooms(GetFilterMeetRoomDto request);
        Task<Response<List<MeetRoomStatusDto>>> GetRoomsOnlineAsync(GetFilterMeetRoomDto request);
        Task<Response<MeetRoom>> UpdateRoomSatusAsync(MeetRoom request);
        Task<Response<MeetRoom>> CreateRoomStatusAsync(MeetRoom request);
    }
}
