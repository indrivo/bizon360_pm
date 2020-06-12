using System;
using System.Collections.Generic;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderList
{
    public class TableHeaderListViewModel
    {
        public IList<TableHeaderDetailModel> TableHeaderList { get; set; }
    }
}
