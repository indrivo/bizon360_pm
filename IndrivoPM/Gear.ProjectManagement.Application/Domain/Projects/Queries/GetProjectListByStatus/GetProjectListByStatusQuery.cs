using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectListByGroup;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectListByStatus
{
    public class GetProjectListByStatusQuery : IRequest<ProjectListViewModel>
    {
        public ICollection<ProjectStatus> Filter { get; set; }

        public bool GetAllProjects { get; set; }
    }
}
