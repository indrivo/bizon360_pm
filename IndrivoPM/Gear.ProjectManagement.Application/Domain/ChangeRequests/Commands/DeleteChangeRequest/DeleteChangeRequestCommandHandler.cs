using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.DeleteChangeRequest
{
    public class DeleteChangeRequestCommandHandler : IRequestHandler<DeleteChangeRequestCommand, Unit>
    {
        private readonly IGearContext _context;

        public DeleteChangeRequestCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteChangeRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ChangeRequests.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ChangeRequest), request.Id);
            }

            _context.ChangeRequests.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
