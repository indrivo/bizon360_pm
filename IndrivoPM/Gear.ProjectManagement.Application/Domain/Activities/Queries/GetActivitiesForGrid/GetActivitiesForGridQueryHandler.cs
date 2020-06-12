using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivitiesForGrid
{
    public class GetActivitiesForGridQueryHandler : IRequestHandler<GetActivitiesForGridQuery, ActivitiesForGrid>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public GetActivitiesForGridQueryHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<ActivitiesForGrid> Handle(GetActivitiesForGridQuery request, CancellationToken cancellationToken)
        {
            var activities = _context.Activities
                .Include(a => a.LoggedTimes)
                .Include(a => a.ActivityType)
                .Include(a => a.Project)
                .Include(a => a.ActivityList)
                .Include(a => a.Assignees)
                .IsInvolved(request, _userAccessor)
                .Select(activity =>
                    new ActivityForGrid
                    {
                        Id = activity.Id,
                        Name = activity.Name,
                        Assignees = activity.Assignees.Select(aa => new AssigneeDto { Id = aa.UserId, FullName = $"{aa.User.FirstName} {aa.User.LastName}"}).ToList(),
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
                    })
                .Where(x => (!request.SprintIds.Any()) || (x.SprintId != null && request.SprintIds.Contains(x.SprintId.Value)));

            return new ActivitiesForGrid
            {
                Activities = await activities.ToListAsync(cancellationToken: cancellationToken)
            };
        }
    }
    public static class ActivityExtensions
    {
        public static IQueryable<Activity> IsInvolved(this IQueryable<Activity> activities, GetActivitiesForGridQuery request, IUserAccessor userAccessor)
        {
            if (request.UserIsInvolved)
            {
                var user = userAccessor.GetUser();
                return activities.Where(a => a.ProjectId == request.ProjectId && a.Assignees.Any(x => x.UserId == Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value)));
            }

            return activities.Where(a => a.ProjectId == request.ProjectId);
        }
    }
}
