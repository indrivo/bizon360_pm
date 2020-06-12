using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportProjectStatuses
{
    public class UserReportProjectStatuses
    {
        public IList<ProjectStatus> ProjectStatuses { get; set; }
    }
}
