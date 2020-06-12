using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTimeByPeriod
{
    public class GetUserLoggedTimeByPeriodQuery : IRequest<UserLoggedTimeListViewModel>
    {
        public Guid UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
