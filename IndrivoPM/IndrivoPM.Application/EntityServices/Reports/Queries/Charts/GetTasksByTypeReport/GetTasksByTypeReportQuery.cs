using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByTypeReport
{
    public class GetTasksByTypeReportQuery : IRequest<ProjectTaskStatisticListViewModel>
    {
        public Guid ProjectId { get; set; }

        public IList<Guid> ActivityTypes { get; set; }

        public bool ShowAllData { get; set; } = false;
    }
}
