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
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateActivity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.CreateActivity
{
    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="context"></param>
        /// <param name="userAccessor"></param>
        public CreateActivityCommandHandler(IMediator mediator, IGearContext context, IUserAccessor userAccessor)
        {
            _mediator = mediator;
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = new Activity
            {
                Description = request.Description,
                ProjectId = request.ProjectId,
                ActivityTypeId = request.ActivityTypeId,
                ActivityListId = request.ActivityListId,
                SprintId = request.SprintId,
                ChnageRequestId = request.ChangeRequestId,
                ActivityPriority = request.Priority,
                ActivityStatus = request.Status,
                Progress = request.Progress,
                StartDate = request.StartDate,
                DueDate = request.DueDate,
                EstimatedHours = request.EstimatedHours,
                Project = await _context.Projects.Include(p => p.ProjectSettings)
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken)
            };

            entity.CreateEnd(request.Id, request.Name, Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            var assigneesList = request.Assignees.Select(assignee => new ActivityAssignee {ActivityId = entity.Id, UserId = assignee}).ToList();

            _context.Activities.Add(entity);
            _context.ActivityAssignees.AddRange(assigneesList);

            var sprint = await _context.Sprints.FindAsync(request.SprintId);
            if (sprint != null)
            {
                sprint.SprintStatus = SprintStatus.OnGoing;
                _context.Sprints.Update(sprint);
            }

            var activityList = await _context.ActivityLists.FindAsync(request.ActivityListId);
            if (activityList != null)
            {
                activityList.ActivityListStatus = ActivityListStatus.OnGoing;
                _context.ActivityLists.Update(activityList);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            if (entity.Project.ProjectSettings.ActivityNotificationOnCreateUpdateCompleteDelete)
            {
                var notification = new AssignedToActivity
                {
                    PrimaryEntityName = entity.Name,
                    PrimaryEntityId = entity.Id,
                    GroupEntityId = entity.ProjectId,
                    GroupEntityName = entity.Project.Name,
                    Recipients = assigneesList.Any() ? assigneesList.Select(aa => aa.User.Email).ToList() : new List<string>(),
                    UserName = user.Identity.Name
                };

                await _mediator.Publish(notification, cancellationToken);
            }

            if (request.ChangeRequestId.HasValue && request.ChangeRequestId.Value != Guid.Empty)
            {
                var changeRequest = await _context.ChangeRequests.FindAsync(request.ChangeRequestId);
                changeRequest.Status = ChangeRequestStatus.Approved;
                changeRequest.ReviewBy = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
                changeRequest.ReviewDate = DateTime.Now;
                _context.ChangeRequests.Update(changeRequest);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
