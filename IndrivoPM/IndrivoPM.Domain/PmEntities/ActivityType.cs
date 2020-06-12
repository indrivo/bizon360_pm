using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Domain.PmEntities
{
    public class ActivityType:BaseModel
    {
        public string Abbreviation { get; set; }

        public ColorBadge ColorBadge { get; set; } = ColorBadge.Blue;

        public int? RowOrder { get; set; }

        public ICollection<Activity> Activities { get; set; }

        public ICollection<TrackerType> TrackerTypes { get; set; }

        public ICollection<ProjectActivityType> ProjectActivityTypes { get; set; }
    }
}
