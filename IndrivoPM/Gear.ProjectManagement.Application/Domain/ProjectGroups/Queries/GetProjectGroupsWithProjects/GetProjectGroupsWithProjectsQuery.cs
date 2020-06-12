using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects
{
    public class GetProjectGroupsWithProjectsQuery : IRequest<ProjectGroupCollection>
    {
        public bool GetAllProjects { get; set; }
    }
}
