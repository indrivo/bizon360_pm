using System;
using System.Linq;
using Gear.Domain.AppEntities;
using Gear.Domain.HrmEntities;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Extensions.EfCore;
using Microsoft.AspNetCore.Identity;

namespace Gear.Persistence
{
    public class IndrivoInitializer
    {
        public static void Initialize(GearContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            var initializer = new IndrivoInitializer();
            initializer.Seed(context, userManager, roleManager);
        }

        public void Seed(GearContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();

            SeedUsers(context, userManager);

            SeedPlatforms(context);

            SeedProjectGroups(context, userManager);

            SeedDepartments(context, userManager);

            SeedBusinessUnits(context, userManager);

        }

        public void SeedUsers(GearContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.Users.Any(u => u.UserName == "admin@indrivo.com"))
            {
                var admin = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "Indrivo",
                    UserName = "admin@indrivo.com",
                    NormalizedUserName = "ADMIN@INDRIVO.COM",
                    Email = "admin@indrivo.com",
                    NormalizedEmail = "admin@indrivo.com"
                };

                var result = userManager.CreateAsync(admin, "@dminPass1").Result;

                if (!result.Succeeded)
                {
                    throw new Exception($"Default user {admin.UserName} cannot be created.");
                }
            }
        }

        public void SeedDepartments(GearContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.Departments.Any(u => !u.IsDeletable)) return;
            var user = userManager.FindByEmailAsync("admin@indrivo.com").Result;

            var invisibleDepartment = new Department
            {
                IsDeletable = false,
            };
            invisibleDepartment.CreateEnd(Guid.NewGuid(),"NotDeletable",user.Id);

            context.Departments.Add(invisibleDepartment);
            context.SaveChanges();
        }

        public void SeedPlatforms(GearContext context)
        {
            context.Instance.Set<Platform>().AddIfNotExists(new Platform()
            {
                Name = "CRM"
            });

            context.Instance.Set<Platform>().AddIfNotExists(new Platform()
            {
                Name = "PM"
            });

            context.Instance.Set<Platform>().AddIfNotExists(new Platform()
            {
                Name = "HRM"
            });

            context.SaveChanges();
        }

        public void SeedBusinessUnits(GearContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.BusinessUnits.Any(u => !u.IsDeletable)) return;
            var user = userManager.FindByEmailAsync("admin@indrivo.com").Result;

            var invisibleBusinessUnit = new BusinessUnit
            {
                IsDeletable = false
            };

            invisibleBusinessUnit.CreateEnd(Guid.NewGuid(), "Uncategorized", user.Id);

            context.BusinessUnits.Add(invisibleBusinessUnit);
            context.SaveChanges();
        }

        public void SeedProjectGroups(GearContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.ProjectGroups.Any(pg => pg.Name == "Uncategorized"))
            {
                var user = userManager.FindByEmailAsync("admin@indrivo.com").Result;

                var defaultProjectGroup = new ProjectGroup
                {
                    IsDeletable = false
                };

                defaultProjectGroup.CreateEnd(Guid.NewGuid(), "Uncategorized", user.Id);

                context.ProjectGroups.Add(defaultProjectGroup);
                context.SaveChanges();
            }
        }
    }
}
