using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetActivityListByProjectReport
{
    public class GetActivityListByProjectReportQuery : IRequest<ActivityListByProjectReportListViewModel>
    {
        public Guid ProjectId { get; set; }

        public List<Guid> ActivityListIds { get; set; }

    }
}
