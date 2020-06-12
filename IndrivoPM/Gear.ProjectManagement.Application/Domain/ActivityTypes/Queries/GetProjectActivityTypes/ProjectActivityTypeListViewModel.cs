using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetProjectActivityTypes
{
    public class ProjectActivityTypeListViewModel
    {
        public IList<ProjectActivityTypeModel> ProjectActivityTypes { get; set; }
            = new List<ProjectActivityTypeModel>();
    }
}
