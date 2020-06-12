using System.Threading;
using System.Threading.Tasks;
using Gear.Identity.Permissions.Services;
using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
    {
        private readonly IPermissionContext _context;

        public UpdateRoleCommandHandler(IPermissionContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _context.RolesToPermissions.FindAsync(request.RoleName);

            role.Update(request.Description,request.Permissions);

            _context.RolesToPermissions.Update(role);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
