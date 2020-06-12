using System;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetLoggedTimeByPeriodList;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralLoggedTimeByPeriodList
{
    public class GetGeneralLoggedTimeByPeriodListQuery : IRequest<LoggedTimeByPeriodListViewModel>
    {
        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
