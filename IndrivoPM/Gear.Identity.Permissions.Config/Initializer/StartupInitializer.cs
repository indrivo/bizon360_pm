using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Identity.Permissions.Domain.Entities;
using Gear.Identity.Permissions.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Gear.Identity.Permissions.Config.Initializer
{
    public class StartupInitializer
    {
        private const string SeedDataDir = "SeedData";

        private const string RoleToPermissionsFilename = "rolestopermissions.json";

        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public StartupInitializer(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task RoleInitialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = services.GetRequiredService<IHostingEnvironment>();

                var pathToRolePermissionData = Path.GetFullPath(Path.Combine(env.WebRootPath, SeedDataDir, RoleToPermissionsFilename));
                var rolesToPermissions = JsonConvert.DeserializeObject<List<RoleToPermissions>>(File.ReadAllText(pathToRolePermissionData));
                var context = services.GetRequiredService<IPermissionContext>();
                foreach (var role in rolesToPermissions)
                {
                    await _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = role.RoleName,
                        PlatformName = role.PlatformName
                    });
                }

                var existingPermissions = context.RolesToPermissions.ToList();
                
                if (!existingPermissions.Any())
                {
                    await context.RolesToPermissions.AddRangeAsync(rolesToPermissions);
                    await context.SaveChangesAsync(CancellationToken.None);
                }

                var admin = await _userManager.FindByNameAsync("admin@indrivo.com");
                await _userManager.AddToRoleAsync(admin, "PM Owner");
            }
        }
    }
}
