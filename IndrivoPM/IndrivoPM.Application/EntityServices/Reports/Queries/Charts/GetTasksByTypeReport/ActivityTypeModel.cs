using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByTypeReport
{
    public class ActivityTypeModel
    {
        public Guid ActivityTypeId { get; set; }

        public string ActivityTypeName { get; set; }

        public int ActivitiesCount { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<Activity>>, ActivityTypeModel>> Projection
        {
            get
            {
                return activity => new ActivityTypeModel
                {
                    ActivityTypeId = activity.Key,
                    ActivityTypeName = activity.Value[0].ActivityType.Name,
                    ActivitiesCount = activity.Value.Count
                };
            }
        }

        public static ActivityTypeModel Create(KeyValuePair<Guid, List<Activity>> activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
