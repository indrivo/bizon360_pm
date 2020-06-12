using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembers
{
    public class GetProjectMembersQuery : IRequest<ApplicationUserListViewModel>
    {
        public Guid Id { get; set; }
        public ICollection<ActivityStatus> Filter { get; set; }
    }
}
