using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportActivityStatuses
{
    public class UserReportActivityStatusesModel
    {
        public IList<ActivityStatus> ActivityStatuses { get; set; }
    }
}
