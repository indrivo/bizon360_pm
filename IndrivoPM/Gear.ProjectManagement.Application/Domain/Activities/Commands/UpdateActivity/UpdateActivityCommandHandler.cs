using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateActivity
{
    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand>
    {
        private readonly IMediator _mediator;
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public UpdateActivityCommandHandler(IMediator mediator, IGearContext context, IUserAccessor userAccessor)
        {
            _mediator = mediator;
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var currentUserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var projectSettings = await _context.ProjectSettings.FirstAsync(x => x.ProjectId == request.ProjectId, cancellationToken);

            var entity = _context.Activities
                .Include(a => a.Assignees).ThenInclude(aa => aa.User)
                .Include(a => a.Project).ThenInclude(p => p.ProjectSettings)
                .Include(a => a.LoggedTimes)
                .First(a => a.Id == request.Id);

            if (projectSettings.ActivityChangeName || request.ProjectManagerId == currentUserId) entity.Name = request.Name;

            entity.Description = request.Description;
            if (projectSettings.ActivityChangeProject || request.ProjectManagerId == currentUserId) entity.ProjectId = request.ProjectId;
            if (projectSettings.ActivityChangeActivityType || request.ProjectManagerId == currentUserId) entity.ActivityTypeId = request.ActivityTypeId;
            if (projectSettings.ActivityChangeActivityList || request.ProjectManagerId == currentUserId) entity.ActivityListId = request.ActivityListId;
            if (projectSettings.ActivityChangeSprint || request.ProjectManagerId == currentUserId) entity.SprintId = request.SprintId;
            if (projectSettings.ActivityChangeProirity || request.ProjectManagerId == currentUserId) entity.ActivityPriority = request.Priority;
            if (projectSettings.ActivityChangeStatus || request.ProjectManagerId == currentUserId) entity.ActivityStatus = request.Status;
            if (projectSettings.ActivityChangeStartDueDate || request.ProjectManagerId == currentUserId)
            {
                entity.StartDate = request.StartDate;
                entity.DueDate = request.DueDate;
            }

            if (projectSettings.ActivityChangeEstimatedTime || request.ProjectManagerId == currentUserId)
            {
                if (entity.EstimatedHours != request.EstimatedHours)
                {
                    var estimatedTime = request.EstimatedHours;
                    var totalActivityLoggedTime = entity.LoggedTimes.Sum(lt => lt.Time);
                    var percentage = (totalActivityLoggedTime * 100) / estimatedTime;

                    entity.Progress = (int) percentage;
                }

                entity.EstimatedHours = request.EstimatedHours;
            }

            var existingAssignees = entity.Assignees.Select(u => u.UserId).ToList();
            var newAssigneeIds = request.Assignees.Except(existingAssignees).ToList();
            var newlyAssignedMembers = await _context.ApplicationUsers
                .Where(x => newAssigneeIds.Contains(x.Id)).ToListAsync(cancellationToken);

            if (request.Assignees != null)
            {
                var viewModelAssignees = request.Assignees;
                var assigneesToDelete = existingAssignees.Except(viewModelAssignees).ToList();
                var assigneesToAdd = viewModelAssignees.Except(existingAssignees).ToList();

                var activityAssigneesToRemove = assigneesToDelete.Select(userId => _context.ActivityAssignees
                        .Where(aa => aa.ActivityId == entity.Id)
                        .SingleOrDefault(aa => aa.UserId == userId))
                    .Where(activityAssigneeToRemove => activityAssigneeToRemove != null).ToList();

                if (activityAssigneesToRemove.Any())
                {
                    _context.ActivityAssignees.RemoveRange(activityAssigneesToRemove);
                }

                if (assigneesToAdd.Any())
                {
                    var assigneesList = assigneesToAdd
                        .Select(userId => new ActivityAssignee {UserId = userId, ActivityId = entity.Id}).ToList();
                    await _context.ActivityAssignees.AddRangeAsync(assigneesList, cancellationToken);
                }
            }
            else
            {
                entity.Assignees.Clear();
            }

            _context.Activities.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            if (entity.Project.ProjectSettings.ActivityNotificationOnCreateUpdateCompleteDelete
                && newlyAssignedMembers.Any())
            {
                await _mediator.Publish(new AssignedToActivity
                {
                    PrimaryEntityId = entity.Id,
                    PrimaryEntityName = entity.Name,
                    GroupEntityId = entity.ProjectId,
                    GroupEntityName = entity.Project.Name,
                    Recipients = newlyAssignedMembers.Select(aa => aa.Email).ToList(),
                    UserName = user.Identity.Name
                }, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
