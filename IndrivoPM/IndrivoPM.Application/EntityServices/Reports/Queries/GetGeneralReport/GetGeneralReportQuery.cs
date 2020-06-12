using System;
using System.Collections.Generic;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGeneralReport;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralReport 
{ 
    public class GetGeneralReportQuery : IRequest<GeneralReportList>
    {
        public List<Guid> GroupsIds { get; set; }
        public List<Guid> ProjectsIds { get; set; }
        public List<Guid> TeamsIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Interval IntervalType { get; set; }
    }
}
