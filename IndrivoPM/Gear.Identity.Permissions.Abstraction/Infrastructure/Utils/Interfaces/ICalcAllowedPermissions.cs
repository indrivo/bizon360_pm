using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gear.Identity.Permissions.Infrastructure.Utils.Interfaces
{
    public interface ICalcAllowedPermissions
    {
        Task<string> CalcPermissionsForUser(IEnumerable<Claim> claims);
    }
}
