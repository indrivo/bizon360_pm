using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Identity.Permissions.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gear.Identity.Permissions.Services
{
    public interface IPermissionContext
    {
        DbSet<RoleToPermissions> RolesToPermissions { get; set; }

        DbSet<ModulesForUser> ModulesForUsers { get; set; }

        DbSet<ApplicationUser> ApplicationUsers { get; set; }

        DbSet<ApplicationRole> Roles { get; set; }

        DbSet<ApplicationUserRole> UserRoles { get; set; }

        DbSet<Platform> Platforms { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
