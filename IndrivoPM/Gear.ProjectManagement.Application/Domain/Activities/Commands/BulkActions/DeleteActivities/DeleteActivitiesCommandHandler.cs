using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.DeleteActivities
{
    public class DeleteActivitiesCommandHandler : IRequestHandler<DeleteActivitiesCommand, Unit>
    {
        private readonly IGearContext _context;

        public DeleteActivitiesCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteActivitiesCommand request, CancellationToken cancellationToken)
        {
            var activitiesToDelete = new List<Activity>();

            foreach (var id in request.ActivitiesById)
            {
                var activity = await _context.Activities.FindAsync(id);

                if (activity == null)
                {
                    throw new NotFoundException(nameof(Activity), id);
                }

                activitiesToDelete.Add(activity);
            }

            if (activitiesToDelete.Any())
            {
                _context.Activities.RemoveRange(activitiesToDelete);

                await _context.SaveChangesAsync(cancellationToken);
            }
            
            return Unit.Value;
        }
    }
}
