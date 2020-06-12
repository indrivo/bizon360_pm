using System;
using System.Collections.Generic;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralSprintListByProjectReport;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetSprintListReportFromSelected
{
    public class GetSprintListReportFromSelectedQuery : IRequest<SprintListGeneralReportListViewModel>
    {
        public Guid Id { get; set; }

        public List<Guid> SprintIds { get; set; }
    }
}
