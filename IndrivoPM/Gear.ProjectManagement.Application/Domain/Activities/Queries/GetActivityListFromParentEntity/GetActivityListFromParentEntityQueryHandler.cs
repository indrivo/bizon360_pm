using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity
{
    public class GetActivityListFromParentEntityQueryHandler : IRequestHandler<GetActivityListFromParentEntityQuery, ActivityListViewModel>
    {
        private readonly IGearContext _context;

        public GetActivityListFromParentEntityQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityListViewModel> Handle(GetActivityListFromParentEntityQuery request, CancellationToken cancellationToken)
        {
            switch (request.ParentType)
            {
                case ActivityParentType.ActivityList:
                    return new ActivityListViewModel
                    {
                        Activities = await _context.Activities
                        .Include(a => a.ActivityType)
                        .Include(a => a.Assignees)
                        .Include(a => a.LoggedTimes)
                        .Include(a => a.Sprint).ThenInclude(s => s.Activities)
                        .Where(a => a.ActivityListId == request.ParentEntityId &&
                                    request.Filter.Contains(a.ActivityStatus))
                        .Skip(request.Skip)
                        .Take(request.Size)
                        .Select(activity => new ActivityLookupModel
                        {
                            Id = activity.Id,
                            Name = activity.Name,
                            Priority = activity.ActivityPriority,
                            ActivityType = activity.ActivityType.Abbreviation,
                            ActivityTypeColor = activity.ActivityType.ColorBadge,
                            EstimatedTime = activity.EstimatedHours ?? 0,
                            LoggedTime = activity.LoggedTimes.Sum(lt => lt.Time),
                            Progress = activity.Progress,
                            Assignees = activity.Assignees.Select(aa => ApplicationUserLookupModel.Create(aa.User)).ToList(),
                            SprintId = activity.SprintId,
                            Sprint = activity.Sprint != null ? activity.Sprint.Name : string.Empty,
                            SprintDueDate = activity.Sprint != null ? activity.Sprint.EndDate : DateTime.MinValue,
                            SprintIsCompleted = activity.Sprint != null && activity.Sprint.Activities.All(a => a.ActivityStatus == ActivityStatus.Completed),
                            DueDate = activity.DueDate,
                            ProjectId = activity.ProjectId,
                            ProjectName = activity.Project != null ? activity.Project.Name : string.Empty,
                            Status = activity.ActivityStatus,
                            ActivityListId = activity.ActivityListId,
                            Number = activity.Number
                        }).ToListAsync(cancellationToken: cancellationToken)
                    };
                case ActivityParentType.UserActivities:
                    return new ActivityListViewModel
                    {
                        Activities = (await _context.Activities
                                .Where(a => a.Assignees.Any(aa => aa.UserId == request.ParentEntityId && request.Filter.Contains(a.ActivityStatus)))
                                .Include(a => a.Project)
                                .Include(a => a.ActivityType)
                                .Include(a => a.Assignees)
                                .Include(a => a.LoggedTimes)
                                .Include(a => a.Sprint)
                                .Skip(request.Skip)
                                .Take(request.Size)
                                .ToListAsync(cancellationToken))
                            .Select(ActivityLookupModel.Create).ToList()
                    };
                case ActivityParentType.Sprint:
                    return new ActivityListViewModel
                    {
                        Activities = (await _context.Activities
                                .Include(a => a.ActivityType)
                                .Include(a => a.Assignees)
                                .Include(a => a.LoggedTimes)
                                .Include(a => a.Sprint).ThenInclude(s => s.Activities)
                                .Where(a => a.SprintId == request.ParentEntityId && request.Filter.Contains(a.ActivityStatus))
                                .Skip(request.Skip)
                                .Take(request.Size)
                                .ToListAsync(cancellationToken))
                            .Select(ActivityLookupModel.Create).ToList()
                    };
                case ActivityParentType.Employee:
                    return new ActivityListViewModel
                    {
                        Activities = (await _context.Activities
                                .Include(a => a.ActivityType)
                                .Include(a => a.Assignees)
                                .Include(a => a.LoggedTimes)
                                .Include(a => a.Sprint).ThenInclude(s => s.Activities)
                                .Where(a => a.Assignees.Any(assignee => assignee.UserId == request.ParentEntityId) 
                                            && request.Filter.Contains(a.ActivityStatus)
                                            && a.ProjectId == request.ProjectId)
                                .Skip(request.Skip)
                                .Take(request.Size)
                                .ToListAsync(cancellationToken))
                            .Select(ActivityLookupModel.Create).ToList()
                        
                    };
                default:
                    return null;
            }
        }
    }
}
