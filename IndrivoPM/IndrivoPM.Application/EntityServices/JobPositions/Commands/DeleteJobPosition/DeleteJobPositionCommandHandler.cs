using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.DeleteJobPosition
{
    public class DeleteJobPositionCommandHandler : IRequestHandler<DeleteJobPositionCommand>
    {
        private readonly IGearContext _context;

        public DeleteJobPositionCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteJobPositionCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Ids)
            {
                var entity = await _context.JobPositions.FindAsync(item);

                if (entity == null) throw new NotFoundException(nameof(JobPosition), item);

                var hasUsers = _context.ApplicationUsers.Any(x => x.JobPositionId == item);

                if (hasUsers)
                {
                    throw new DeleteFailureException(nameof(JobPosition), item,
                        "There are existing entities associated with this job position.");
                }

                _context.JobPositions.Remove(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
