using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeDetails
{
    public class TrackerTypeDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ActivityTypeId { get; set; }

        public string ActivityTypeName { get; set; }

        public bool Active { get; set; }

        public static Expression<Func<TrackerType, TrackerTypeDetailModel>> Projection
        {
            get
            {
                return trackerType => new TrackerTypeDetailModel
                {
                    Id = trackerType.Id,
                    Name = trackerType.Name,
                    Active = trackerType.Active,
                    ActivityTypeId = trackerType.ActivityTypeId,
                    ActivityTypeName = trackerType.ActivityType != null && trackerType.ActivityType.Name != null
                        ? trackerType.ActivityType.Name
                        : ""
                };
            }
        }
        public static TrackerTypeDetailModel Create(TrackerType trackerType)
        {
            return Projection.Compile().Invoke(trackerType);
        }
    }
}
