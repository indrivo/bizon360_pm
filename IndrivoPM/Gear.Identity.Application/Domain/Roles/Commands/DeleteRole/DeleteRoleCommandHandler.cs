using System.Threading;
using System.Threading.Tasks;
using Gear.Identity.Permissions.Services;
using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
    {
        private readonly IPermissionContext _context;

        public DeleteRoleCommandHandler(IPermissionContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {

            foreach (var item in request.RoleNames)
            {
                var entity = await _context.RolesToPermissions.FindAsync(item);

                if (entity != null)
                {
                    _context.RolesToPermissions.Remove(entity);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
