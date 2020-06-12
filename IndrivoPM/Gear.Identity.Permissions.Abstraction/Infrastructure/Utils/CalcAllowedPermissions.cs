using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gear.Identity.Permissions.Infrastructure.Utils.Interfaces;
using Gear.Identity.Permissions.Services;
using Microsoft.EntityFrameworkCore;

namespace Gear.Identity.Permissions.Infrastructure.Utils
{
    public class CalcAllowedPermissions : ICalcAllowedPermissions
    {
        private readonly IPermissionContext _dbContext;

        public CalcAllowedPermissions(IPermissionContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// This is called if the Permissions that a user needs calculating.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public async Task<string> CalcPermissionsForUser(IEnumerable<Claim> claims)
        {
            var usersRoles = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value)
                .ToList();

            //This gets all the permissions, with a distinct to remove duplicates
            var permissionsForUser = await _dbContext.RolesToPermissions.Where(x => usersRoles.Contains(x.RoleName))
                .SelectMany(x => x.GetPermissionsInRole)
                .Distinct()
                .ToListAsync();

            //TODO:FEATURE_Improvement: can comment out the below code to apply the modules restrictions as well

            //we get the modules this user is allowed to see
            //var userModules =
            //    _dbContext.ModulesForUsers.Find(Guid.Parse(claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value))
            //        ?.AllowedPaidForModules ?? PaidForModules.None;



            //Now we remove permissions that are linked to modules that the user has no access to
            var filteredPermissions =
                from permission in permissionsForUser
                //let moduleAttr = typeof(Domain.Resources.Permissions).GetMember(permission.ToString())[0]
                //    .GetCustomAttribute<LinkedToModuleAttribute>()
                //where moduleAttr == null || userModules.HasFlag(moduleAttr.PaidForModule)
                select permission;

            return filteredPermissions.PackPermissionsIntoString();
        }
    }
}
