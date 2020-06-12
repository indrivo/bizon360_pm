using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByPriorityReport
{
    public class GetTasksByPriorityReportQuery : IRequest<ProjectTaskStatisticListViewModel>
    {
        public Guid ProjectId { get; set; }

        public IList<ActivityPriority> ActivityPriorities { get; set; }

        public bool ShowAllData { get; set; } = false;
    }
}
