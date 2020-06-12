using System;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeList;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetMonthlyLogsByActivity
{
    public class GetMonthlyLogsByActivityQuery : IRequest<LoggedTimeListViewModel>
    {
        public Guid EmployeeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
