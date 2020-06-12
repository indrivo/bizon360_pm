using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.UpdateOrderTrackerType
{
    public class UpdateOrderTrackerTypeCommandHandler : IRequestHandler<UpdateOrderTrackerTypeCommand>
    {
        private readonly IGearContext _context;

        public UpdateOrderTrackerTypeCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrderTrackerTypeCommand request, CancellationToken cancellationToken)
        {
            var count = 1;

            foreach (var item in request.TrackerTypeIds)
            {
                var entity = await _context.TrackerTypes.FindAsync(item);

                if (entity == null) throw new NotFoundException(nameof(TrackerType), item);

                entity.RowOrder = count;

                _context.TrackerTypes.Update(entity);

                count++;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
