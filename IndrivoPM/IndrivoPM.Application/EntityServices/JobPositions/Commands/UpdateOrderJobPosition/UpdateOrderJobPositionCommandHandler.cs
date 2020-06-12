using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.UpdateOrderJobPosition
{
    public class UpdateOrderJobPositionCommandHandler : IRequestHandler<UpdateOrderJobPositionCommand>
    {
        private readonly IGearContext _context;

        public UpdateOrderJobPositionCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrderJobPositionCommand request, CancellationToken cancellationToken)
        {
            var count = 1;

            foreach (var item in request.jobPositionIds)
            {
                var entity = await _context.JobPositions.FindAsync(item);

                if (entity == null) throw new NotFoundException(nameof(JobPosition), item);

                entity.RowOrder = count;

                _context.JobPositions.Update(entity);

                count++;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
