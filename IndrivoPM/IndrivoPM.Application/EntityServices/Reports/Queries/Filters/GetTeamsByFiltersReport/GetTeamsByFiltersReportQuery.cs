using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport
{
    public class GetTeamsByFiltersReportQuery : IRequest<TeamsFilteredReportListViewModel>
    {
        public IList<Guid> EmployeeIds { get; set; }

        public IList<Guid> ActivityTypeIds { get; set; }

        public IList<ActivityStatus> ActivityStatuses { get; set; }

        public IList<Guid> ProjectIds { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
