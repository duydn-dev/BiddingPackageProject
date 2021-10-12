using AutoMapper;
using Neac.Common.Dtos;
using Neac.Common.Dtos.PositionDtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserCreateDto, User>().ForMember(x => x.UserRoles, g => g.Ignore());
            CreateMap<User, UserCreateDto>();

            CreateMap<MeetRoom, MeetRoom>().ForMember(x => x.MeetRoomId, g => g.Ignore());

            CreateMap<UserPosition, PositonGetDropdownViewDto>();
            CreateMap<PositonGetDropdownViewDto, UserPosition>().ForMember(x => x.Users, g => g.Ignore());
            
        }
    }
}
