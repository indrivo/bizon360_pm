using System;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralSprintListByProjectReport
{
    public class ActivityOfSprintLookupModel
    {
        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public int ActivityNumber { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public string Assignees { get; set; }

        public float? EstimatedTime { get; set; }

        public float? LoggedTime { get; set; }

        public DateTime CreateTime { get; set; }

        public static Expression<Func<Activity, ActivityOfSprintLookupModel>> Projection
        {
            get
            {
                return activity => new ActivityOfSprintLookupModel
                {
                    ActivityId = activity.Id,
                    ActivityName = activity.Name,
                    ActivityNumber = activity.Number,
                    ActivityStatus = activity.ActivityStatus,
                    Assignees = string.Join(", ", activity.Assignees.Select(x => $"{x.User.FirstName ?? string.Empty} {x.User.LastName ?? string.Empty}")),
                    EstimatedTime = activity.EstimatedHours ?? 0f,
                    LoggedTime = activity.LoggedTimes.Sum(x => x.Time),
                    CreateTime = activity.CreatedTime
                };
            }
        }
        public static ActivityOfSprintLookupModel Create(Activity activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
