using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Gear.Identity.Permissions.Domain.Resources;

namespace Gear.Identity.Permissions.Infrastructure.Utils
{
    public static class PermissionExtensions
    {
        /// <summary>
        /// This returns true if the current user has the permission
        /// is used on UI to hide/show elements
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static bool UserHasThisPermission(this ClaimsPrincipal user, Domain.Resources.Permissions permission)
        {
            var permissionClaim =
                user?.Claims.SingleOrDefault(x => x.Type == PermissionConstants.PackedPermissionClaimType);
            return permissionClaim?.Value.UnpackPermissionsFromString().Contains(permission) == true;
        }
    }
}
