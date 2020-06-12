using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.DateTimeExtensions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTimeByPeriod
{
    public class GetUserLoggedTimeByPeriodQueryHandler : IRequestHandler<GetUserLoggedTimeByPeriodQuery, UserLoggedTimeListViewModel>
    {
        private readonly IGearContext _context;

        public GetUserLoggedTimeByPeriodQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UserLoggedTimeListViewModel> Handle(GetUserLoggedTimeByPeriodQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ApplicationUsers.FindAsync(request.UserId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), request.UserId);
            }

            var loggedTimes = (await _context.LoggedTimes.Where(x => x.UserId == request.UserId &&
                x.DateOfWork.IsInRangeInclusive(request.StartDate, request.EndDate))
                .Include(x => x.Activity).ThenInclude(xx => xx.Project)
                .Include(x => x.Tracker)
                .Include(x => x.User)
                .ToListAsync(cancellationToken)).Select(UserLoggedTimeLookupModel.Create).OrderByDescending(x => x.DateOfWork).ToList();

            var totalLoggedTime = loggedTimes.Sum(x => x.Time);

            return new UserLoggedTimeListViewModel
            {
                LoggedTimes = loggedTimes,
                TotalLoggedTime = totalLoggedTime
            };
        }
    }
}
