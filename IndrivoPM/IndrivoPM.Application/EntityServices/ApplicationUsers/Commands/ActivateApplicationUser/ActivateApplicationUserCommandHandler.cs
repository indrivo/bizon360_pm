using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.ActivateApplicationUser
{
    public class ActivateApplicationUserCommandHandler : IRequestHandler<ActivateApplicationUserCommand>
    {
        private readonly IGearContext _context;

        public ActivateApplicationUserCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ActivateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Ids)
            {
                var user = await _context.ApplicationUsers.FindAsync(item);
                user.Active = request.Active;
                _context.ApplicationUsers.Update(user);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
