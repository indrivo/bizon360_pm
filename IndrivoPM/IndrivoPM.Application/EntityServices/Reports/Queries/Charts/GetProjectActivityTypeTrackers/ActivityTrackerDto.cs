using System;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectActivityTypeTrackers
{
    public class ActivityTrackerDto
    {
        public Guid TrackerTypeId { get; set; }

        public string TrackerName { get; set; }

        public float LoggedTime { get; set; }
    }
}
