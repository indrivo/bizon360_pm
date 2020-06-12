using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByEmployeesReport
{
    public class GetTasksByEmployeesReportQuery : IRequest<ProjectTasksView>
    {
        public Guid ProjectId { get; set; }

        public IList<Guid> EmployeeIds { get; set; }

        public bool ShowAllData { get; set; }
    }
}
