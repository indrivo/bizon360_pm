using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupList
{
    public class ProjectGroupListViewModel
    {
        public ICollection<ProjectGroupLookupModel> ProjectGroups { get; set; }
    }
}
