using System;
using System.ComponentModel;
using System.Linq;

namespace Gear.Identity.Permissions.Infrastructure.Utils
{
    public static class PermissionChecker
    {
        public static bool ThisPermissionIsAllowed(this string packedPermissions, string permissionName)
        {
            var usersPermissions = packedPermissions.UnpackPermissionsFromString().ToArray();

            if (!Enum.TryParse(permissionName, true, out Domain.Resources.Permissions permissionToCheck))
                throw new InvalidEnumArgumentException($"{permissionName} could not be converted to a {nameof(Domain.Resources.Permissions)}.");

            return usersPermissions.Contains(permissionToCheck);
        }
    }
}
