using System;
using System.Collections.Generic;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport
{
    public class GetGeneralByFiltersReportQuery : IRequest<GeneralFilteredReportListViewModel>
    {
        public List<Guid> GroupsIds { get; set; }

        public List<Guid> ProjectsIds { get; set; }

        public List<Guid> TeamsIds { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
