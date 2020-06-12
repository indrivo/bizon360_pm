using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectLoggedTimeReportList
{
    public class GetProjectLoggedTimeReportListQuery : IRequest<ProjectLoggedTimeReportListViewModel>
    {
        public List<Guid> ProjectIds { get; set; } = new List<Guid>();

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
