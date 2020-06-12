using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.DeleteSprintCommand
{
    public class DeleteSprintCommandHandler : IRequestHandler<DeleteSprintCommand>
    {
        private readonly IGearContext _context;

        public DeleteSprintCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSprintCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Sprints.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Sprint), request.Id);
            }

            _context.Sprints.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
