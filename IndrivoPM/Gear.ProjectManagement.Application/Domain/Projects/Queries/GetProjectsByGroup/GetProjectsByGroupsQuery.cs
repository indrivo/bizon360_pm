using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsByGroup
{
    public class GetProjectsByGroupsQuery : IRequest<ProjectsDto>
    {
        public List<Guid> GroupIds { get; set; }
    }
}
