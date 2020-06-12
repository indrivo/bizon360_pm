using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Identity.Permissions.Services;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gear.Identity.Manager.Domain.Roles.Queries.GetRoleDetails
{
    public class GetRoleDetailQueryHandler : IRequestHandler<GetRoleDetailQuery, RoleDetailModel>
    {
        private readonly IPermissionContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetRoleDetailQueryHandler(UserManager<ApplicationUser> userManager, IPermissionContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<RoleDetailModel> Handle(GetRoleDetailQuery request, CancellationToken cancellationToken)
        {
            var role = await _context.RolesToPermissions.FirstAsync(x => x.RoleName == request.RoleName,cancellationToken);

            if (role == null) return null;

            var usersInRole = _userManager.GetUsersInRoleAsync(role.RoleName);

            return new RoleDetailModel
            {
                Name = role.RoleName,
                Description = role.Description,
                Platform = role.PlatformName,
                Active = role.Active,
                Permissions = role.GetPermissionsInRole,
                UserList = usersInRole.Result.Select(x => ApplicationUserLookupModel.Create(x)).ToList() 
            };
        }
    }
}
