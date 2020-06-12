using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList
{
    public class TrackerTypeLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public int? RowOrder { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }


        public static Expression<Func<TrackerType, TrackerTypeLookupModel>> Projection
        {
            get
            {
                return trackerType => new TrackerTypeLookupModel
                {
                    Id = trackerType.Id,
                    Name = trackerType.Name,
                    Active = trackerType.Active,
                    RowOrder = trackerType.RowOrder,
                    CreatedTime = trackerType.CreatedTime,
                    ModifiedTime = trackerType.ModifyTime
                };
            }
        }

        public static TrackerTypeLookupModel Create(TrackerType trackerType)
        {
            return Projection.Compile().Invoke(trackerType);
        }
    }
}
