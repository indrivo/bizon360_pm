using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.AssignActivitiesToSprint
{
    public class AssignActivitiesToSprintCommandHandler : IRequestHandler<AssignActivitiesToSprintCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public AssignActivitiesToSprintCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(AssignActivitiesToSprintCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var activities = new List<Activity>();
            var sprint = await _context.Sprints.FindAsync(request.SprintId);

            foreach (var id in request.ActivitiesById)
            {
                var activity = await _context.Activities.FindAsync(id);

                if (activity == null)
                {
                    throw new NotFoundException(nameof(Activity), id);
                }

                activity.SprintId = request.SprintId;
                activity.DueDate = sprint.EndDate;
                

                activities.Add(activity);
            }

            if (activities.Any())
            {
                _context.Activities.UpdateRange(activities);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
