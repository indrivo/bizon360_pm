using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects
{
    public class ProjectGroupCollection
    {
        public ICollection<ProjectGroupLookupModel> ProjectGroups { get; set; }
    }
}
