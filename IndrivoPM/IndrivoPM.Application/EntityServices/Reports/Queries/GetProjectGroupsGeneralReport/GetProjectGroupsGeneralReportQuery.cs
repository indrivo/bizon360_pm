using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGroupsGeneralReport
{
    public class GetProjectGroupsGeneralReportQuery : IRequest<ProjectGroupsGeneralReportListViewModel>
    {
        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
