using System.Linq;
using System.Threading.Tasks;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Configurations;
using Gear.Identity.Permissions.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Gear.Identity.Permissions.Services
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permissionsClaim =
                context.User.Claims.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);

            // If user does not have the scope claim, get out of here
            if (permissionsClaim == null)
                return Task.CompletedTask;

            if (permissionsClaim.Value.ThisPermissionIsAllowed(requirement.PermissionName))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
