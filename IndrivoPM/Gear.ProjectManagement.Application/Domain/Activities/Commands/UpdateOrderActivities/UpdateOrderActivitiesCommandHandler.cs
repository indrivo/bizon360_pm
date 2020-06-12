using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateOrderActivities
{
    internal class UpdateOrderActivitiesCommandHandler : IRequestHandler<UpdateOrderActivitiesCommand>
    {
        private readonly IGearContext _context;

        public UpdateOrderActivitiesCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrderActivitiesCommand request, CancellationToken cancellationToken)
        {
            var count = 1;

            foreach (var item in request.ActivitiesIds)
            {
                var entity = await _context.Activities.FindAsync(item);

                if (entity == null) throw new NotFoundException(nameof(ActivityType), item);

                entity.RowOrder = count;

                _context.Activities.Update(entity);

                count++;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
