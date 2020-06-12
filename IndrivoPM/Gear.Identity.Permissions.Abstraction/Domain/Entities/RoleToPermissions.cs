using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Gear.Identity.Permissions.Infrastructure.Utils;
using Gear.Identity.Permissions.Services;

namespace Gear.Identity.Permissions.Domain.Entities
{
    /// <summary>
    /// Database role
    /// </summary>
    public class RoleToPermissions
    {
        private RoleToPermissions() { }

        /// <summary>
        /// This creates the Role with its permissions
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <param name="permissions"></param>
        /// <param name="platformName"></param>
        public RoleToPermissions(string roleName, string description, ICollection<Resources.Permissions> permissions, string platformName, DateTime modified)
        {
            RoleName = roleName;
            PlatformName = platformName;
            Modified = modified;
            Update(description, permissions);
        }

        /// <summary>
        /// Converted/Serialized list of role permission
        /// </summary>
        public string PermissionsInRole { get; set; }


        /// <summary>
        /// ShortName of the role
        /// </summary>
        public string RoleName { get; private set; }


        /// <summary>
        /// Name of the platform the role is assigned to
        /// </summary>
        public string PlatformName { get; private set; }


        /// <summary>
        /// A human-friendly description of what the Role does
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Creation DateTime
        /// </summary>
        public DateTime Modified { get; private set; }


        /// <summary>
        /// Check if role is actively in use
        /// </summary>
        public bool Active { get; private set; } = true;


        /// <summary>
        /// Deactivate role
        /// <remarks>Save changes after operation</remarks>
        /// </summary>
        public void Deactivate(IPermissionContext context)
        {
            var role = context.RolesToPermissions.Find(this.RoleName);
            role.Active = false;
            role.Modified = DateTime.Now;
        }

        /// <summary>
        /// Activate role
        /// <remarks>Save changes after operation</remarks>
        /// </summary>
        public void Activate(IPermissionContext context)
        {
            var role = context.RolesToPermissions.Find(this.RoleName);
            role.Active = true;
            role.Modified = DateTime.Now;
        }

        /// <summary>
        /// This returns the list of permissions in this role
        /// </summary>
        public IEnumerable<Resources.Permissions> GetPermissionsInRole => PermissionsInRole.UnpackPermissionsFromString();


        public void Update(string description, ICollection<Resources.Permissions> permissions)
        {
            Description = description;
            Modified = DateTime.Now;
            if (permissions == null || !permissions.Any())
                throw new InvalidOperationException("There should be at least one permission associated with a role.");

            PermissionsInRole = permissions.PackPermissionsIntoString();
        }
    }
}
