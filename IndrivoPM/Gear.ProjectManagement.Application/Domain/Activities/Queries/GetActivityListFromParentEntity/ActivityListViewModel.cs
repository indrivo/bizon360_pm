using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity
{
    public class ActivityListViewModel
    {
        public ICollection<ActivityLookupModel> Activities { get; set; }

        //public bool HasCompletedActivities { get; set; }
    }
}
