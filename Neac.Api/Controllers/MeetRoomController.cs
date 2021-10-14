using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.Common.Dtos.MeetRooms;
using Neac.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý phòng họp")]
    public class MeetRoomController : ControllerBase
    {
        private readonly IMeetRoomRepository _meetRoomRepository;
        public MeetRoomController(IMeetRoomRepository meetRoomRepository)
        {
            _meetRoomRepository = meetRoomRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("meet-rooms")]
        public async Task<Response<List<MeetRoom>>> GetFilter([FromQuery]string filter)
        {
            var request = JsonConvert.DeserializeObject<GetFilterMeetRoomDto>(filter);
            return await _meetRoomRepository.GetMeetRooms(request);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("meet-rooms-online")]
        public async Task<Response<List<MeetRoomStatusDto>>> GetRoomsOnlineAsync([FromQuery] string filter)
        {
            var request = JsonConvert.DeserializeObject<GetFilterMeetRoomDto>(filter);
            return await _meetRoomRepository.GetRoomsOnlineAsync(request);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("update-status")]
        public async Task<Response<MeetRoom>> UpdateRoomSatusAsync([FromBody] MeetRoom request)
        {
            return await _meetRoomRepository.UpdateRoomSatusAsync(request);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create-room")]
        public async Task<Response<MeetRoom>> CreateRoomStatusAsync([FromBody] MeetRoom request)
        {
            return await _meetRoomRepository.CreateRoomStatusAsync(request);
        }
    }
}
