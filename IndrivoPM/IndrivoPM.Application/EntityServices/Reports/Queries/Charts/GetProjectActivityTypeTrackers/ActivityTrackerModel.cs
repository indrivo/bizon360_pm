using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectActivityTypeTrackers
{
    public class ActivityTrackerModel
    {
        public Guid ActivityTrackerId { get; set; }

        public string ActivityTrackName { get; set; }

        public float TotalLoggedTime { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ActivityTrackerDto>>, ActivityTrackerModel>> Projection
        {
            get
            {
                return activity => new ActivityTrackerModel
                {
                    ActivityTrackerId = activity.Key,
                    ActivityTrackName = activity.Value[0].TrackerName,
                    TotalLoggedTime = activity.Value.Sum(x => x.LoggedTime)
                };
            }
        }

        public static ActivityTrackerModel Create(KeyValuePair<Guid, List<ActivityTrackerDto>> activity)
        {
            return Projection.Compile().Invoke(activity);
        }

        public static Expression<Func<List<ActivityTrackerDto>, ActivityTrackerModel>> ListProjection
        {
            get
            {
                return activity => new ActivityTrackerModel
                {
                    ActivityTrackerId = activity[0].TrackerTypeId,
                    ActivityTrackName = activity[0].TrackerName,
                    TotalLoggedTime = activity.Sum(x => x.LoggedTime)
                };
            }
        }

        public static ActivityTrackerModel Create(List<ActivityTrackerDto> activity)
        {
            return ListProjection.Compile().Invoke(activity);
        }
    }
}
