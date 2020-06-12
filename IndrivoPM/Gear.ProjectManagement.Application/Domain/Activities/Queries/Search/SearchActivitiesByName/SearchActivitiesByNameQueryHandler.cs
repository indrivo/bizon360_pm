using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivitiesForGrid;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByName
{
    public class SearchActivitiesByNameQueryHandler : IRequestHandler<SearchActivitiesByNameQuery, ActivitiesForGrid>
    {
        private readonly IGearContext _context;
        public SearchActivitiesByNameQueryHandler(IGearContext context)
        {
            _context = context;
        }
        public async Task<ActivitiesForGrid> Handle(SearchActivitiesByNameQuery request, CancellationToken cancellationToken)
        {
            var activities = _context.Activities
                        .Include(x => x.LoggedTimes)
                        .Include(x => x.ActivityType)
                        .Include(x => x.Project)
                        .Include(x => x.ActivityList)
                        .Include(a => a.Assignees)
                        .Where(a => a.ProjectId == request.ProjectId && a.Name.ToLower().Contains(request.SearchValue.ToLower()))
                .Select(activity =>
                            new ActivityForGrid
                            {
                                Id = activity.Id,
                                Name = activity.Name,
                                Assignees = activity.Assignees.Select(aa => new AssigneeDto { Id = aa.UserId, FullName = $"{aa.User.FirstName} {aa.User.LastName}" }).ToList(),
                                Priority = activity.ActivityPriority,
                                Status = activity.ActivityStatus,
                                Progress = activity.Progress,
                                ActivityType = activity.ActivityType,
                                EstimatedHours = activity.EstimatedHours,
                                LoggedTime = activity.LoggedTimes.Sum(lt => lt.Time),
                                StartDate = activity.StartDate,
                                DueDate = activity.DueDate,
                                ProjectId = activity.ProjectId,
                                ProjectName = activity.Project.Name,
                                ActivityListId = activity.ActivityListId,
                                ActivityListName = activity.ActivityList == null ? "None" : activity.ActivityList.Name,
                                SprintId = activity.SprintId,
                                RowOrder = activity.RowOrder
                            });

            return new ActivitiesForGrid
            {
                Activities = await activities.ToListAsync(cancellationToken: cancellationToken)
            };
}
    }
}
