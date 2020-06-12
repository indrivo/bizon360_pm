using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeDetails
{
    public class ActivityTypeDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public ColorBadge ColorBadge { get; set; }

        public Dictionary<Guid, string> TrackerTypes { get; set; }

        public static Expression<Func<ActivityType, ActivityTypeDetailModel>> Projection
        {
            get
            {
                return activityType => new ActivityTypeDetailModel
                {
                    Id = activityType.Id,
                    Name = activityType.Name,
                    Abbreviation = activityType.Abbreviation,
                    ColorBadge = activityType.ColorBadge,
                    TrackerTypes = activityType.TrackerTypes != null
                        ? activityType.TrackerTypes.ToDictionary(t => t.Id, t => t.Name)
                        : new Dictionary<Guid, string>()
                };
            }
        }
        public static ActivityTypeDetailModel Create(ActivityType activityType)
        {
            return Projection.Compile().Invoke(activityType);
        }
    }
}
