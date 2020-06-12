using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQuery : IRequest<ProjectsDto>
    {
    }
}
