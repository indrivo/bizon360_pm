using System;
using System.IO;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Identity.Permissions.Config.Initializer;
using Gear.Notifications.Infrastructure.Persistance;
using Gear.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Bizon360
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    await Task.Run(() =>
                    {
                        var notificationContext = scope.ServiceProvider.GetRequiredService<GearNotificationsContext>();
                        if (notificationContext.Database.EnsureCreated())
                        {
                            notificationContext.Database.Migrate();
                        }
                    });

                    var context = await Task.Run(() => scope.ServiceProvider.GetService<IGearContext>());

                    var userManager = await Task.Run(() => scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>());

                    var roleManager = await Task.Run(() => scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>());

                    var concreteContext = (GearContext)context;
                    await Task.Run(() =>
                    {
                        if (concreteContext.Database.EnsureCreated())
                        {
                            concreteContext.Database.Migrate();
                        }
                        IndrivoInitializer.Initialize(concreteContext, userManager, roleManager);
                    });
                    
                    var initializer = new StartupInitializer(roleManager, userManager);
                    await initializer.RoleInitialize(scope.ServiceProvider);

                    if (concreteContext.Database.EnsureCreated())
                    {
                        concreteContext.Database.Migrate();
                    }
                    await ReportsInitializer.Initialize(concreteContext, userManager);
                }
                catch (Exception e)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e.Message, "An error occurred while migrating or initializing the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddNLog(new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build());
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                })
                .UseSentry()
                .UseStartup<Startup>();
    }
}
