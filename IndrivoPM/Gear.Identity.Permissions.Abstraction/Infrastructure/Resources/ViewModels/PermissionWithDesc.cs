using System;

namespace Gear.Identity.Permissions.Infrastructure.Resources.ViewModels
{
    public class PermissionWithDesc
    {
        public PermissionWithDesc(string permissionName, string description, string linkedToModule)
        {
            PermissionName = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            LinkedToModule = linkedToModule;
        }

        public string PermissionName { get; set; }

        public string Description { get; set; }

        public string LinkedToModule { get; set; }
    }
}
