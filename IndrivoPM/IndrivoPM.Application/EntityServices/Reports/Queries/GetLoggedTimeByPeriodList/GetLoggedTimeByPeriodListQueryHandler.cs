using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetLoggedTimeByPeriodList
{
    public class GetLoggedTimeByPeriodListQueryHandler : IRequestHandler<GetLoggedTimeByPeriodListQuery, LoggedTimeByPeriodListViewModel>
    {
        private readonly IGearContext _context;

        public GetLoggedTimeByPeriodListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<LoggedTimeByPeriodListViewModel> Handle(GetLoggedTimeByPeriodListQuery request, CancellationToken cancellationToken)
        {
            var result = new LoggedTimeByPeriodListViewModel();

            var users = new List<UserLoggedTimeByPeriodLookupModel>();

            foreach (var userId in request.UserIds)
            {
                var userLoggedEstimatedTimeStats = await _context.LoggedTimes.Where(x => x.UserId == userId &&
                    x.DateOfWork.Date >= request.StartDate.Date 
                    && x.DateOfWork.Date <= request.DueDate.Date)
                    .Include(x => x.Activity).ThenInclude(x => x.Assignees).ToListAsync(cancellationToken);

                //var totalEstimatedTimeByUser = userLoggedEstimatedTimeStats.Sum(x => x?.Activity?.EstimatedHours ?? 0f);

                // Gets all activities where user is assigned.
                var assignedActivities = _context.ApplicationUsers.Include(x => x.Activities).ThenInclude(x => x.Activity).First(x => x.Id == userId);

                // Filter user activities by range time.
                var filterByDate = assignedActivities.Activities.Select(x => x.Activity)
                    .Where(x => x.StartDate >= request.StartDate.Date && x.DueDate <= request.DueDate.Date );

                // Calculate estimated time for activities.
                var totalEstimatedTimeByUser = filterByDate.Sum(x => x?.EstimatedHours ?? 0f);

                var totalLoggedTimeByUser = userLoggedEstimatedTimeStats.Sum(x => x?.Time ?? 0f);

                var user = await _context.ApplicationUsers.FindAsync(userId);
                if (user != null)
                {
                    users.Add(new UserLoggedTimeByPeriodLookupModel
                    {
                        Name = user.FirstName + ' ' + user.LastName,
                        UserEstimatedTimeByPeriod = totalEstimatedTimeByUser,
                        UserLoggedTimeByPeriod = totalLoggedTimeByUser
                    });
                }

                result.TotalEstimatedTime += totalEstimatedTimeByUser;
                result.TotalLoggedTime += totalLoggedTimeByUser;
            }

            result.UsersLoggedEstimatedTime = users.OrderBy(x => x.Name).ToList();

            return result;
        }
    }
}
