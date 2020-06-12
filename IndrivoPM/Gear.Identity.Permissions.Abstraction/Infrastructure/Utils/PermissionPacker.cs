using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Gear.Identity.Permissions.Infrastructure.Utils
{
    public static class PermissionPacker
    {
        public const char PackType = 'H';
        public const int PackedSize = 4;

        public static string FormDefaultPackPrefix()
        {
            return $"{PackType}{PackedSize:D1}-";
        }

        public static string PackPermissionsIntoString(this IEnumerable<Domain.Resources.Permissions> permissions)
        {
            return permissions.Aggregate(FormDefaultPackPrefix(), (s, permission) => s + ((int)permission).ToString("X4"));
        }

        public static IEnumerable<int> UnpackPermissionValuesFromString(this string packedPermissions)
        {
            var packPrefix = FormDefaultPackPrefix();
            if (packedPermissions == null)
                throw new ArgumentNullException(nameof(packedPermissions));
            if (!packedPermissions.StartsWith(packPrefix))
                throw new InvalidOperationException("The format of the packed permissions is wrong" +
                                                    $" - should start with {packPrefix}");

            var index = packPrefix.Length;
            while (index < packedPermissions.Length)
            {
                yield return int.Parse(packedPermissions.Substring(index, PackedSize), NumberStyles.HexNumber);
                index += PackedSize;
            }
        }

        public static IEnumerable<Domain.Resources.Permissions> UnpackPermissionsFromString(this string packedPermissions)
        {
            return packedPermissions.UnpackPermissionValuesFromString().Select(x => ((Domain.Resources.Permissions)x));
        }

        public static Domain.Resources.Permissions? FindPermissionViaName(this string permissionName)
        {
            return Enum.TryParse(permissionName, out Domain.Resources.Permissions permission)
                ? (Domain.Resources.Permissions?)permission
                : null;
        }

    }
}
