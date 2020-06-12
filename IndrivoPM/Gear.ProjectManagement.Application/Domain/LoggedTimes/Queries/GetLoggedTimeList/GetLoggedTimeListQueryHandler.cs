using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeList
{
    public class GetLoggedTimeListQueryHandler : IRequestHandler<GetLoggedTimeListQuery, LoggedTimeListViewModel>
    {
        private readonly IGearContext _context;

        public GetLoggedTimeListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<LoggedTimeListViewModel> Handle(GetLoggedTimeListQuery request, CancellationToken cancellationToken)
        {
            return new LoggedTimeListViewModel()
            {
                LoggedTimes = (await _context.LoggedTimes.Where(x => x.ActivityId == request.ActivityId)
                .Include(x => x.Tracker)
                .Include(x => x.Activity)
                .Include(x => x.User)
                .ToListAsync(cancellationToken)).Select(LoggedTimeLookupModel.Create).OrderByDescending(x => x.DateOfWork).ToList()
            };
        }
    }
}
