using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGroupsGeneralReport
{
    public class ProjectGroupsGeneralReportListViewModel
    {
        public float TotalEstimatedTine { get; set; } = 0f;

        public float TotalLoggedTime { get; set; } = 0f;

        public List<ProjectGroupsGeneralReportLookupModel> ProjectGroups { get; set; }
            = new List<ProjectGroupsGeneralReportLookupModel>();
    }
}
