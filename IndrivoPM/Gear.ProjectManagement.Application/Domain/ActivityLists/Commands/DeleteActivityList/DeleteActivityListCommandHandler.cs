using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.DeleteActivityList
{
    public class DeleteActivityListCommandHandler : IRequestHandler<DeleteActivityListCommand>
    {
        private readonly IGearContext _context;

        public DeleteActivityListCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteActivityListCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ActivityLists.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(ActivityList), request.Id);
            }

            var hasActivities = _context.Activities.Any(a => a.ActivityListId == entity.Id);
            if (hasActivities)
            {
                throw new DeleteFailureException(nameof(ActivityList), request.Id, "There are existing entities associated with this activity list.");
            }

            _context.ActivityLists.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
