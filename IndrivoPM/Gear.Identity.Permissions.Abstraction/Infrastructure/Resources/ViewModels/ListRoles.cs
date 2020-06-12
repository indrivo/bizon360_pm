using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Identity.Permissions.Services;

namespace Gear.Identity.Permissions.Infrastructure.Resources.ViewModels
{
    public class ListRoles
    {
        private readonly IPermissionContext _applicationDbContext;

        public ListRoles(IPermissionContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<RolesListDto> ListRolesWithPermissionsExplained()
        {
            foreach (var roleToPermissions in _applicationDbContext.RolesToPermissions)
            {
                var permissionsWithDesc =
                    from permission in roleToPermissions.GetPermissionsInRole
                    let displayAttr = typeof(Domain.Resources.Permissions).GetMember(permission.ToString())[0]
                        .GetCustomAttribute<DisplayAttribute>()
                    let moduleAttr = typeof(Domain.Resources.Permissions).GetMember(permission.ToString())[0]
                        .GetCustomAttribute<LinkedToModuleAttribute>()

                    select new PermissionWithDesc(permission.ToString(), displayAttr?.Description, moduleAttr?.PaidForModule.ToString());
                yield return new RolesListDto(roleToPermissions.RoleName,roleToPermissions.Description, permissionsWithDesc.ToList(), roleToPermissions.Modified, roleToPermissions.Active);
            }
        }
    }
}
