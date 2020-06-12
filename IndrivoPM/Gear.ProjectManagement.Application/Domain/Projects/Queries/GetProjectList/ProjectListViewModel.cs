using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectList
{
    public class ProjectListViewModel
    {
        public IList<ProjectLookupModel> Projects { get; set; }
    }
}