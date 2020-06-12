using System;
using System.Collections.Generic;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetUserReportHeaders
{
    public class UserReportHeaderListViewModels
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public Guid ReportId { get; set; }

        public string ReportName { get; set; }

        public IList<TableHeaderDetailModel> TableHeaders { get; set; }
    }
}
