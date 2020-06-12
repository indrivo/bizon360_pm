using System;
using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList
{
    public class TrackerTypeListViewModel
    {
        public IList<TrackerTypeLookupModel> Trackers { get; set; }

        public Guid ActivityTypeId { get; set; }

    }
}
