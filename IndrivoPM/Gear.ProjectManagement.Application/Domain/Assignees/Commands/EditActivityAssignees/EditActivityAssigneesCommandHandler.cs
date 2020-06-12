using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateActivity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Assignees.Commands.EditActivityAssignees
{
    public class EditActivityAssigneesCommandHandler : IRequestHandler<EditActivityAssigneesCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public EditActivityAssigneesCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(EditActivityAssigneesCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var activity = await _context.Activities
                .Include(a => a.Assignees)
                .Include(x => x.Project)
                .ThenInclude(x => x.ProjectSettings)
                .FirstAsync(p => p.Id == request.Id, cancellationToken);

            var existingAssignees = activity.Assignees.Select(u => u.UserId).ToList();
            var newAssigneeIds = request.Users.Except(existingAssignees).ToList();
            var newlyAssignedMembers = await _context.ApplicationUsers
                .Where(x => newAssigneeIds.Contains(x.Id)).ToListAsync(cancellationToken);

            if (request.Users != null)
            {
                var viewModelAssignees = request.Users.ToList();
                var assigneesToDelete = existingAssignees.Except(viewModelAssignees).ToList();
                var assigneesToAdd = viewModelAssignees.Except(existingAssignees).ToList();

                var activityAssigneesToRemove = assigneesToDelete.Select(userId => _context.ActivityAssignees
                        .Where(ass => ass.ActivityId == activity.Id)
                        .SingleOrDefault(ass => ass.UserId == userId))
                    .Where(assigneToRemove => assigneToRemove != null).ToList();

                if (activityAssigneesToRemove.Any())
                {
                    _context.ActivityAssignees.RemoveRange(activityAssigneesToRemove);
                }

                if (assigneesToAdd.Any())
                {
                    var assignees = assigneesToAdd
                        .Select(userId => new ActivityAssignee {UserId = userId, ActivityId = activity.Id}).ToList();
                    await _context.ActivityAssignees.AddRangeAsync(assignees, cancellationToken);
                }
            }
            else
            {
                activity.Assignees.Clear();
            }

            _context.Activities.Update(activity);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            if (activity.Project.ProjectSettings.ActivityNotificationOnCreateUpdateCompleteDelete
                && newlyAssignedMembers.Any())
            {
                await _mediator.Publish(new AssignedToActivity
                {
                    PrimaryEntityId = activity.Id,
                    PrimaryEntityName = activity.Name,
                    GroupEntityId = activity.ProjectId,
                    GroupEntityName = activity.Project.Name,
                    Recipients = newlyAssignedMembers.Select(aa => aa.Email).ToList(),
                    UserName = user.Identity.Name
                }, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
