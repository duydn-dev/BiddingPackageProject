using AutoMapper;
using Neac.Common.Dtos;
using Neac.Common.Dtos.BiddingPackage;
using Neac.Common.Dtos.DocumentDtos;
using Neac.Common.Dtos.PositionDtos;
using Neac.Common.Dtos.ProjectDtos;
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

            CreateMap<BiddingPackage, BiddingPackageDto>()
                .ForMember(n => n.Order, g => g.Ignore());
            CreateMap<BiddingPackageDto, BiddingPackage>()
                .ForMember(n => n.BiddingPackageProjects, src => src.Ignore())
                .ForMember(n => n.Documents, src => src.Ignore());
            CreateMap<BiddingPackage, BiddingPackageByIdDto>()
                .ForMember(n => n.Documents, g => g.MapFrom(m => m.Documents));
            CreateMap<BiddingPackageByIdDto, BiddingPackage>()
                .ForMember(n => n.Documents, g => g.MapFrom(m => m.Documents));

            CreateMap<BiddingPackageProject, BiddingPackageProjectDto>();
            CreateMap<BiddingPackageProjectDto, BiddingPackageProject>()
                .ForMember(n => n.Project, src => src.Ignore());

            CreateMap<Project, ProjectGetListDto>()
                .ForMember(dest => dest.BiddingPackageDtos, opt => opt.Ignore());
            CreateMap<ProjectGetListDto, Project>();
            CreateMap<Project, ProjectGetByIdDto>()
                .ForMember(n => n.BiddingPackageProjectDtos, src => src.MapFrom(n => n.BiddingPackageProjects));
            CreateMap<ProjectGetByIdDto, Project>()
               .ForMember(n => n.BiddingPackageProjects, src => src.MapFrom(n => n.BiddingPackageProjectDtos));

            CreateMap<ProjectFlow, ProjectFlow>()
                .ForMember(n => n.ProjectFlowId, g => g.Ignore());

            CreateMap<Document, DocumentDto>();
            CreateMap<DocumentDto, Document>()
                .ForMember(n => n.BiddingPackage, g => g.Ignore());
            
            CreateMap<ProjectFlowCreateDto, ProjectFlow>();
            CreateMap<ProjectFlow, ProjectFlowCreateDto>();
        }
    }
}
