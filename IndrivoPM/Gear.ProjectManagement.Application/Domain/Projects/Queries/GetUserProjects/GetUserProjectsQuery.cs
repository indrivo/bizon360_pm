using System;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetAllProjects;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetUserProjects
{
    public class GetUserProjectsQuery : IRequest<ProjectsDto>
    {
        public Guid UserId { get;set; }
    }
}
