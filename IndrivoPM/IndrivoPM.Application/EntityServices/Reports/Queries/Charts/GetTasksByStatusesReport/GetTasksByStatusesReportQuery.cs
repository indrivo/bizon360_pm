using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByStatusesReport
{
    public class GetTasksByStatusesReportQuery : IRequest<ProjectTaskStatisticListViewModel>
    {
        public Guid ProjectId { get; set; }

        public IList<ActivityStatus> Statuses { get; set; }

        public bool ShowAllData { get; set; } = false;
    }
}
