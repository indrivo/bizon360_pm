using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport
{
    public class GetProjectsByFiltersReportQuery : IRequest<ProjectFilteredReportListViewModel>
    {
        public IList<Guid> ProjectIds { get; set; } 
            = new List<Guid>();

        public IList<Guid> ActivityTypes { get; set; } 
            = new List<Guid>();

        public IList<ActivityStatus> ActivityStatuses { get; set; }
            = new List<ActivityStatus>();

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
