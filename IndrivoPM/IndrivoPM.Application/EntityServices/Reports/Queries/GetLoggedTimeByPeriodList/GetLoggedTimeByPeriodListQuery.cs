using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetLoggedTimeByPeriodList
{
    public class GetLoggedTimeByPeriodListQuery : IRequest<LoggedTimeByPeriodListViewModel>
    {
        public List<Guid> UserIds { get; set; } = new List<Guid>();

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
