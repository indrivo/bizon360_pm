using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByStatusesReport
{
    public class ActivityStatusModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ActivityStatus ActivityStatus { get; set; }

        public int ActivitiesCount { get; set; }

        public static Expression<Func<KeyValuePair<ActivityStatus, List<Activity>>, ActivityStatusModel>> Projection
        {
            get
            {
                return activity => new ActivityStatusModel
                {
                    ActivityStatus = activity.Key,
                    ActivitiesCount = activity.Value.Count
                };
            }
        }

        public static ActivityStatusModel Create(KeyValuePair<ActivityStatus, List<Activity>> activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
