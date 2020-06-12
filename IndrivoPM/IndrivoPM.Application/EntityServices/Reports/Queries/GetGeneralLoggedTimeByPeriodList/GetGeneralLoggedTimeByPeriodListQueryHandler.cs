using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.DateTimeExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetLoggedTimeByPeriodList;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralLoggedTimeByPeriodList
{
    public class GetGeneralLoggedTimeByPeriodListQueryHandler : IRequestHandler<GetGeneralLoggedTimeByPeriodListQuery, LoggedTimeByPeriodListViewModel>
    {
        private readonly IGearContext _context;

        public GetGeneralLoggedTimeByPeriodListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<LoggedTimeByPeriodListViewModel> Handle(GetGeneralLoggedTimeByPeriodListQuery request, CancellationToken cancellationToken)
        {
            var result = new LoggedTimeByPeriodListViewModel();

            var users = new List<UserLoggedTimeByPeriodLookupModel>();
            var userIds = _context.ApplicationUsers.Select(x => x.Id)?.ToList();

            foreach (var userId in userIds)
            {
                var userLoggedEstimatedTimeStats = await _context.LoggedTimes.Where(x => x.UserId == userId &&
                    x.DateOfWork.IsInRangeInclusive(request.StartDate, request.DueDate))
                    .Include(x => x.Activity).ToListAsync();

                var totalEstimatedTimeByUser = userLoggedEstimatedTimeStats.Sum(x => x?.Activity?.EstimatedHours ?? 0f);
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
