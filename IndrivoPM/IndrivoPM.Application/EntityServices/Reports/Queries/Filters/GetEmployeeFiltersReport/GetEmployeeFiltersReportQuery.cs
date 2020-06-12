using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport
{
    public class GetEmployeeFiltersReportQuery : IRequest<EmployeeFilteredReportListViewModel>
    {
        public IList<Guid> UserIds { get; set; }

        public IList<Guid> ProjectIds { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
