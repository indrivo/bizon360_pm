using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.DeleteActivity
{
    public class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand>
    {
        private readonly IGearContext _context;

        public DeleteActivityCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            _context.Activities.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
