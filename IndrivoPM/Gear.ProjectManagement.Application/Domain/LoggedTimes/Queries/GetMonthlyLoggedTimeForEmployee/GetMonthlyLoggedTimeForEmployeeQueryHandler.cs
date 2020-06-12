using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.DateTimeExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetMonthlyLoggedTimeForEmployee
{
    public class GetMonthlyLoggedTimeForEmployeeQueryHandler : IRequestHandler<GetMonthlyLoggedTimeForEmployeeQuery, MonthlyReportViewModel>
    {
        private readonly IGearContext _context;

        public GetMonthlyLoggedTimeForEmployeeQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<MonthlyReportViewModel> Handle(GetMonthlyLoggedTimeForEmployeeQuery request, CancellationToken cancellationToken)
        {
            var endDate = request.EndDate ?? request.Date.AddDays(DateTime.DaysInMonth(request.Date.Year, request.Date.Month));

            var numberOfDays = (endDate - request.Date).Days + 1;

            var timeLogs = (await _context.LoggedTimes
                    .Include(lt => lt.Activity)
                    .Where(lt => lt.CreatedBy == request.EmployeeId && lt.DateOfWork.IsInRangeInclusive(request.Date, endDate))
                    .Include(lt => lt.Activity)
                    .ToListAsync(cancellationToken))
                .Select(TimeLogModel.Create)
                .ToList();

            var report = new MonthlyReportViewModel();
            report.NumberOfDays = numberOfDays;
            report.DaysOfWeek = request.Date.GetDays(request.EndDate);
            report.TimeLogs = GetTimeLogs(timeLogs, numberOfDays, request.Date, endDate);
            report.TotalPerMonth = timeLogs.Sum(l => l.Time);
            report.TotalEstimated = timeLogs.GroupBy(l => l.ActivityId).Select(l => l.First()).Sum(l => l.Estimated);


            return report;
        }

        private float[] GetTimeLogs(ICollection<TimeLogModel> timeLogs, int numberOfDays, DateTime startDate, DateTime endDate)
        {
            var array = new float[numberOfDays];

            foreach (var timeLog in timeLogs)
            {
                var i = (timeLog.Date - startDate).Days;
                array[i] += timeLog.Time;
            }

            return array;
        }
    }
}
