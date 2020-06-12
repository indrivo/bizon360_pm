using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectList
{
    public class GetProjectListQuery : IRequest<ProjectListViewModel>
    {
        public bool GetAllProjects { get; set; }
    }
}