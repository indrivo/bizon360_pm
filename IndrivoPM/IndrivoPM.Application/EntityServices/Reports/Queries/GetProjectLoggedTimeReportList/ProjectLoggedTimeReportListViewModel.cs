
using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectLoggedTimeReportList
{
    public class ProjectLoggedTimeReportListViewModel
    {
        public float TotalEstimatedTime { get; set; }

        public float TotalLoggedTime { get; set; }

        public IList<ProjectLoggedTimeReportLookupModel> ProjectReports { get; set; }
    }
}
