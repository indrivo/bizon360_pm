using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByActivityList
{
    public class ActivityListSearchViewModel
    {
        public ICollection<ActivityListIncludeModel> ActivityLists { get; set; }
    }
}