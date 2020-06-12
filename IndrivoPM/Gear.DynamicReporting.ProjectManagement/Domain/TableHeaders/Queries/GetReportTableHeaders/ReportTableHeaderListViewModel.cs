using System;
using System.Collections.Generic;
using System.Text;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetReportTableHeaders
{
    public class ReportTableHeaderListViewModel
    {
        public Guid ReportId { get; set; }

        public string ReportName { get; set; }

        public IList<TableHeaderDetailModel> TableHeaders { get; set; }
    }
}
