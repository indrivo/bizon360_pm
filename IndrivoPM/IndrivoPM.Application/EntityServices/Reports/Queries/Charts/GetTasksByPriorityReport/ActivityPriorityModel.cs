using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByPriorityReport
{
    public class ActivityPriorityModel
    {
        public ActivityPriority ActivityPriority { get; set; }

        public int ActivitiesCount { get; set; }

        public static Expression<Func<KeyValuePair<ActivityPriority, List<Activity>>, ActivityPriorityModel>> Projection
        {
            get
            {
                return activity => new ActivityPriorityModel
                {
                    ActivityPriority = activity.Key,
                    ActivitiesCount = activity.Value.Count
                };
            }
        }

        public static ActivityPriorityModel Create(KeyValuePair<ActivityPriority, List<Activity>> activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
