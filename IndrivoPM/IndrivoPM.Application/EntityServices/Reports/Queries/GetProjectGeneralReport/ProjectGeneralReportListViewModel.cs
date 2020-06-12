using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGeneralReport
{
    public class ProjectGeneralReportListViewModel
    {
        public string ProjectName { get; set; }

        public float TotalEstimatedTime { get; set; }

        public float TotalLoggedTime { get; set; }

        public List<ProjectGeneralReportLookupModel> Activities { get; set; }
            = new List<ProjectGeneralReportLookupModel>();
    }
}
