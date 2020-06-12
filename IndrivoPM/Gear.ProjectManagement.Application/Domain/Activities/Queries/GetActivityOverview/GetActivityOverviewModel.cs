using System;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityOverview
{
 public class ActivityOverviewModel
    {
        public Guid Id { get; set; }
        public ActivityStatus Status { get; set; }
        public int Progress { get; set; }

        public float? LoggedTime { get; set; }



        public static Expression<Func<Activity, ActivityOverviewModel>> Projection
        {
            get
            {
                return activity => new ActivityOverviewModel
                {
                    Id = activity.Id,
                    Status = activity.ActivityStatus,
                    Progress = activity.Progress,
                    LoggedTime = activity.LoggedTimes.Sum(x => x.Time)
                };
            }
        }

        public static ActivityOverviewModel Create(Activity activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
