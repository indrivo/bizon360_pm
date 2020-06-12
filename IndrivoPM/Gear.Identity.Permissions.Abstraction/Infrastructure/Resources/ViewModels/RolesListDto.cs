using System;
using System.Collections.Generic;

namespace Gear.Identity.Permissions.Infrastructure.Resources.ViewModels
{
    public class RolesListDto
    {
        public RolesListDto(string roleName, string description, IList<PermissionWithDesc> permissionsWithDesc, DateTime created, bool active)
        {
            RoleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
            Description = description;
            PermissionsWithDesc = permissionsWithDesc ?? throw new ArgumentNullException(nameof(permissionsWithDesc));
            CreatedTime = created;
            Active = active;
        }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public IList<PermissionWithDesc> PermissionsWithDesc { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool Active { get; set; }
    }
}
