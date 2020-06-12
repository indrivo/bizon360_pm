using System;
using System.Linq;
using AutoMapper;
using Bizon360.DataContracts;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserDetails;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateActivity;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails;

namespace Bizon360.AutoMapper
{
    public class ApplicationToViewModelProfile : Profile
    {
        public ApplicationToViewModelProfile()
        {
            CreateMap<ApplicationUserDetailModel, ApplicationUserDetailViewModel>()
                .ForMember(dest => dest.Teams,
                    act => act.MapFrom(src => string.Join(", ", src.Teams.Select(x => x.Value).ToArray()))
                )
                .ForMember(dest => dest.RoleNames,
                    act => act.MapFrom(src => string.Join(", ", src.RoleNames.Select(x => x).ToArray()))
                );

            CreateMap<ActivityDetailModel, UpdateActivityCommand>()
                .ForMember(dest => dest.Assignees, act => act.MapFrom(src => src.Assignee));
        }
    }
}
