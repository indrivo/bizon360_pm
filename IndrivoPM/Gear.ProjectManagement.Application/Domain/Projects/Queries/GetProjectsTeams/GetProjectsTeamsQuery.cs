using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsTeams
{
    public class GetProjectsTeamsQuery : IRequest<TeamsDto>
    {
        public List<Guid> ProjectsIds { get; set; }
    }
}
