using System.Threading;
using System.Threading.Tasks;
using Gear.Identity.Permissions.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Identity.Manager.Domain.Roles.Commands.ActivateRole
{
    public class ActivateRoleCommandHandler : IRequestHandler<ActivateRoleCommand>
    {
        private readonly IPermissionContext _context;

        public ActivateRoleCommandHandler(IPermissionContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ActivateRoleCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.RoleNames)
            {
                var entity = await _context.RolesToPermissions
                    .FirstAsync(x => x.RoleName == item, cancellationToken);

                if (entity == null) continue;

                if (request.Active)
                    entity.Activate(_context);
                else
                    entity.Deactivate(_context);

                _context.RolesToPermissions.Update(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
