using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGeneralReport
{
    public class GetProjectGeneralReportQuery : IRequest<ProjectGeneralReportListViewModel>
    {
        public Guid ProjectId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
