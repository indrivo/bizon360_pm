using System;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;

namespace Gear.Domain.PmEntities
{
    public class LoggedTime : BaseModel
    {
        public float Time { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid ActivityId { get; set; }

        public Activity Activity { get; set; }

        public Guid TrackerId { get; set; }

        public TrackerType Tracker { get; set; }

        public DateTime DateOfWork { get; set; } = DateTime.Now;
    }
}
