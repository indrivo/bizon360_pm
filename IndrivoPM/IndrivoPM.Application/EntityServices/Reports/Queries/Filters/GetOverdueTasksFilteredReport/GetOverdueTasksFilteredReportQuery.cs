using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport
{
    public class GetOverdueTasksFilteredReportQuery : IRequest<OverdueTasksFilteredReportViewModel>
    {
        public IList<Guid> ProjectIds { get; set; }

        public IList<Guid> AssigneeIds { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public bool AllowNullDueDateAsDeadline { get; set; } = false;
    }
}
