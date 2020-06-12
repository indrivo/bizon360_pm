using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.BulkActions.ChangeProjectActivityTypeStates
{
    public class ChangeProjectActivityTypeStatesCommandHandler : IRequestHandler<ChangeProjectActivityTypeStatesCommand>
    {
        private readonly IGearContext _context;

        public ChangeProjectActivityTypeStatesCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ChangeProjectActivityTypeStatesCommand request, CancellationToken cancellationToken)
        {
            var entities = await _context.ProjectActivityTypes
                .Where(x => x.ProjectId == request.ProjectId
                            && request.ActivityTypeIds.Contains(x.ActivityTypeId)).ToListAsync(cancellationToken);

            foreach (var entity in entities)
            {
                entity.Active = request.Active;
            }

            _context.ProjectActivityTypes.UpdateRange(entities);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
