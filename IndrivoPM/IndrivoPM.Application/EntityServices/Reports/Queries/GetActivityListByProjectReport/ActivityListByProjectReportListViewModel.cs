using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetActivityListByProjectReport
{
    public class ActivityListByProjectReportListViewModel
    {
        public string ProjectName { get; set; }

        public List<string> ActivityTypes { get; set; } = new List<string>();

        public List<ActivityListByProjectReportLookupModel> ActivityList { get; set; }
            = new List<ActivityListByProjectReportLookupModel>();
    }
}
