using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.DateTimeExtensions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeList;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetMonthlyLogsByActivity
{
    public class GetMonthlyLogsByActivityQueryHandler : IRequestHandler<GetMonthlyLogsByActivityQuery, LoggedTimeListViewModel>
    {
        private readonly IGearContext _context;

        public GetMonthlyLogsByActivityQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<LoggedTimeListViewModel> Handle(GetMonthlyLogsByActivityQuery request, CancellationToken cancellationToken)
        {
            var endDate = request.EndDate ?? request.StartDate.AddDays(DateTime.DaysInMonth(request.StartDate.Year, request.StartDate.Month));

            return new LoggedTimeListViewModel
            {
                LoggedTimes = (await _context.LoggedTimes
                    .Include(lt => lt.Activity).ThenInclude(a => a.Project)
                    .Include(lt => lt.Tracker)
                    .Where(lt => lt.CreatedBy == request.EmployeeId
                                 && lt.DateOfWork.IsInRangeInclusive(request.StartDate, endDate))
                    .ToListAsync(cancellationToken)).Select(LoggedTimeLookupModel.Create).ToList()
            };
        }
    }
}
