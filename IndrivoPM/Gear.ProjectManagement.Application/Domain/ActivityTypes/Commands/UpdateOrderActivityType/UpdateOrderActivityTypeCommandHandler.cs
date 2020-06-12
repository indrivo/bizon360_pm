using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.UpdateOrderActivityType
{
    internal class UpdateOrderActivityTypeCommandHandler : IRequestHandler<UpdateOrderActivityTypeCommand>
    {
        private readonly IGearContext _context;

        public UpdateOrderActivityTypeCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrderActivityTypeCommand request, CancellationToken cancellationToken)
        {
            var count = 1;

            foreach (var item in request.ActivityTypeIds)
            {
                var entity = await _context.ActivityTypes.FindAsync(item);

                if (entity == null) throw new NotFoundException(nameof(ActivityType), item);

                entity.RowOrder = count;

                _context.ActivityTypes.Update(entity);

                count++;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
