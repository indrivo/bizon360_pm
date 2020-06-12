using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembers
{
    public class ApplicationUserListViewModel
    {
        public ICollection<ProjectMemberLookupModel> Users { get; set; }
    }
}
