using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeList;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeListByProject
{
    public class GetLoggedTimeListByProjectQueryHandler : IRequestHandler<GetLoggedTimeListByProjectQuery, LoggedTimeListViewModel>
    {
        private readonly IGearContext _context;

        public GetLoggedTimeListByProjectQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<LoggedTimeListViewModel> Handle(GetLoggedTimeListByProjectQuery request, CancellationToken cancellationToken)
        {
            return new LoggedTimeListViewModel
            {
                LoggedTimes = (await _context.LoggedTimes.Where(lt => lt.Activity.ProjectId == request.ProjectId)
                        .Include(lt => lt.Activity)
                        .Include(lt => lt.Tracker)
                        .Include(lt => lt.User)
                        .ToListAsync(cancellationToken))
                    .Select(LoggedTimeLookupModel.Create)
                    .OrderByDescending(lt => lt.DateOfWork)
                    .ToList()
            };
        }
    }
}
