using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeList
{
    public class ActivityTypeLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public ColorBadge ColorBadge { get; set; }

        public bool Active { get; set; }

        public List<TrackerTypeLookupModel> TrackerTypes { get; set; }

        public static Expression<Func<ActivityType, ActivityTypeLookupModel>> Projection
        {
            get
            {
                return activityType => new ActivityTypeLookupModel
                {
                    Id = activityType.Id,
                    Name = activityType.Name,
                    Abbreviation = activityType.Abbreviation,
                    ColorBadge = activityType.ColorBadge,
                    Active = activityType.Active,
                    TrackerTypes = activityType.TrackerTypes != null ? activityType.TrackerTypes.Select(x => TrackerTypeLookupModel.Create(x)).ToList() : null
                };
            }
        }

        public static ActivityTypeLookupModel Create(ActivityType activityType)
        {
            return Projection.Compile().Invoke(activityType);
        }
    }
}
