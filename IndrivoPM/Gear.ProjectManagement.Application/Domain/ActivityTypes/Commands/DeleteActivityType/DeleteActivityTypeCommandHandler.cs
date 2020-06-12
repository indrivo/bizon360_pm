using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.DeleteActivityType
{
    public class DeleteActivityTypeCommandHandler : IRequestHandler<DeleteActivityTypeCommand>
    {
        private readonly IGearContext _context;

        public DeleteActivityTypeCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteActivityTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ActivityTypes.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ActivityType), request.Id);
            }

            var hasActivities = _context.Activities.Any(x => x.ActivityTypeId == entity.Id);

            if (hasActivities)
            {
                throw new DeleteFailureException(nameof(ActivityType), request.Id,
                    "There are existing entities associated with this activity type.");
            }

            _context.ActivityTypes.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
