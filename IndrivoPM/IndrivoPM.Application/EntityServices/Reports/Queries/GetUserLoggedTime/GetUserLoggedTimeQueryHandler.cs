using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.DateTimeExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTime
{
    public class GetUserLoggedTimeQueryHandler : IRequestHandler<GetUserLoggedTimeQuery, UserLoggedTimeListViewModel>
    {
        private readonly IGearContext _context;

        public GetUserLoggedTimeQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UserLoggedTimeListViewModel> Handle(GetUserLoggedTimeQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.LoggedTimes.Where(x => x.UserId == request.EmployeeId &&
                x.DateOfWork.IsInRangeInclusive(request.StartDate, request.DueDate))
                .Include(x => x.Activity).ThenInclude(xx => xx.Project)
                .Include(x => x.Tracker)
                .Include(x => x.User)
                .ToListAsync(cancellationToken);

            var loggedTimes = query.Select(UserLoggedTimeLookupModel.Create).OrderByDescending(x => x.DateOfWork).ToList();

            var user = await _context.ApplicationUsers.FindAsync(request.EmployeeId);

            return new UserLoggedTimeListViewModel
            {
                UserName = user != null ? user.FirstName + ' ' + user.LastName : "Undefined",
                LoggedTimes = loggedTimes,
                TotalLoggedTime = loggedTimes.Sum(x => x.Time),
                TotalEstimatedTime = query.Sum(x => x.Activity?.EstimatedHours ?? 0f)
            };
        }
    }
}
