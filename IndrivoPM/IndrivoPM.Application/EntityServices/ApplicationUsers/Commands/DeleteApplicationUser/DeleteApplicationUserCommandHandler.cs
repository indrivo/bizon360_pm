using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.DeleteApplicationUser
{
    public class DeleteApplicationUserCommandHandler : IRequestHandler<DeleteApplicationUserCommand>
    {
        private readonly IGearContext _context;

        public DeleteApplicationUserCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ApplicationUsers.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), request.Id);
            }

            _context.ApplicationUsers.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
