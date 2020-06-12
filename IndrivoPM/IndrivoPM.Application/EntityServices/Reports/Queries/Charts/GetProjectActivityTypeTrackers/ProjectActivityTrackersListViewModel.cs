using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectActivityTypeTrackers
{
    public class ProjectActivityTrackersListViewModel
    {
        public string ActivityTypeName { get; set; }

        public IList<ActivityTrackerModel> ActivityTrackers { get; set; }
    }
}
