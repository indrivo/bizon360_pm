using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityLists
{
    public class GetActivityListsQueryHandler : IRequestHandler<GetActivityListsQuery, ActivityListsViewModel>
    {
        private readonly IGearContext _context;

        public GetActivityListsQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityListsViewModel> Handle(GetActivityListsQuery request, CancellationToken cancellationToken)
        {
            bool hasFilters = request.Filter != null && request.Filter.Any();

            var activityListDto = hasFilters
                ? await _context.ActivityLists
                    .Include(al => al.Activities).ThenInclude(a => a.LoggedTimes)
                    .Where(al => al.ProjectId == request.ProjectId)
                    .Select(al => new ActivityListLookupModel
                    {
                        Id = al.Id,
                        Name = al.Name,
                        CompletedActivitiesCount = al.Activities.Count(a => a.ActivityStatus == ActivityStatus.Completed),
                        ActivitiesCount = al.Activities.Count(a => request.Filter.Any(tag => a.ActivityStatus == tag)),
                        TotalActivitiesCount = al.Activities.Count,
                        EstimatedTime = al.Activities.Any() ? al.Activities.Sum(a => a.EstimatedHours ?? 0) : 0,
                        LoggedTime = al.Activities.Any()
                            ? al.Activities.Select(a => a.LoggedTimes.Sum(lt => lt.Time)).Sum()
                            : 0,
                        IsCompleted = al.ActivityListStatus == ActivityListStatus.Completed
                    })
                    .OrderBy(al => al.Name).ToListAsync(cancellationToken: cancellationToken)
                : await _context.ActivityLists
                    .Include(al => al.Activities).ThenInclude(a => a.LoggedTimes)
                    .Where(al => al.ProjectId == request.ProjectId)
                    .Select(al => new ActivityListLookupModel
                    {
                        Id = al.Id,
                        Name = al.Name,
                        CompletedActivitiesCount =
                            al.Activities.Count(a => a.ActivityStatus == ActivityStatus.Completed),
                        ActivitiesCount = al.Activities.Count,
                        TotalActivitiesCount = al.Activities.Count,
                        EstimatedTime = al.Activities.Any() ? al.Activities.Sum(a => a.EstimatedHours ?? 0) : 0,
                        LoggedTime = al.Activities.Any()
                            ? al.Activities.Select(a => a.LoggedTimes.Sum(lt => lt.Time)).Sum()
                            : 0,
                        IsCompleted = al.ActivityListStatus == ActivityListStatus.Completed
                    })
                    .OrderBy(al => al.Name).ToListAsync(cancellationToken: cancellationToken);

            return new ActivityListsViewModel
            {
                ActivityLists = activityListDto
            };
        }
    }
}
