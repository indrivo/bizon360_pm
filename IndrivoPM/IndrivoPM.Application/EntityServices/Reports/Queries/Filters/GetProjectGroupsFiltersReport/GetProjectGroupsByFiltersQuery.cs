using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport
{
    public class GetProjectGroupsByFiltersQuery : IRequest<ProjectGroupFilteredReportListViewModel>
    {
        public IList<Guid> AssigneeIds { get; set; } = new List<Guid>();

        public IList<Guid> ActivityTypeIds { get; set; } = new List<Guid>(); 

        public IList<ProjectStatus> ProjectStatuses { get; set; } = new List<ProjectStatus>();

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public Interval IntervalType { get; set; }
    }
}
