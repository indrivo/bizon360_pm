using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetMonthlyLoggedTimeForEmployee
{
    public class GetMonthlyLoggedTimeForEmployeeQuery : IRequest<MonthlyReportViewModel>
    {
        public Guid EmployeeId { get; set; }

        public DateTime Date { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
