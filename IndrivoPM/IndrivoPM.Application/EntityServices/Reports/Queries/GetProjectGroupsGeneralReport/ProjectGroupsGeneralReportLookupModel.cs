using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGroupsGeneralReport
{
    public class ProjectGroupsGeneralReportLookupModel
    {
        public string ProjectGroupName { get; set; }

        public float TotalEstimatedTime { get; set; } = 0f;

        public float TotalLoggedTime { get; set; } = 0f;

        public List<ProjectReportLookupModel> Projects { get; set; }
            = new List<ProjectReportLookupModel>();
    }
}
