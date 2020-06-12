using System;
using System.Collections.Generic;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportList
{
    public class UserReportListViewModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public IList<UserReportModel> UserReports { get; set; }
    }
}
