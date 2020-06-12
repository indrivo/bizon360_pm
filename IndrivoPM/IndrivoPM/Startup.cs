using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Bizon360.AutoMapper;
using Bizon360.Infrastructure.Claims;
using Bizon360.Models;
using Bizon360.Utils;
using FluentValidation.AspNetCore;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Infrastructure;
using Gear.CloudStorage.Abstractions.Service;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Common.Extensions.HttpClient;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportGuidList;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsByName;
using Gear.Identity.Manager.Domain.Roles.Commands.ActivateRole;
using Gear.Identity.Permissions.Infrastructure.Configurations;
using Gear.Identity.Permissions.Infrastructure.Utils;
using Gear.Identity.Permissions.Infrastructure.Utils.Interfaces;
using Gear.Identity.Permissions.Services;
using Gear.Localizer.Extensions;
using Gear.Localizer.Localizer;
using Gear.Localizer.Service;
using Gear.Localizer.Service.Abstractions;
using Gear.MailSender.Services;
using Gear.Manager.Core.Infrastructure;
using Gear.Manager.Core.Infrastructure.AutoMapper;
using Gear.Manager.Core.Infrastructure.MediatorResultHandler;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Settings;
using Gear.Notifications.Infrastructure.Hubs;
using Gear.Notifications.Infrastructure.Persistance;
using Gear.Notifications.Infrastructure.Resolvers;
using Gear.OneDriveCloudStorage.Service;
using Gear.Persistence;
using Gear.Persistence.Settings;
using Gear.ProjectManagement.Manager.Background;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.CreateProject;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectDetails;
using Gear.Sstp.Abstractions.Infrastructure;
using Gear.Sstp.Abstractions.Service;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SmartBreadcrumbs.Extensions;

namespace Bizon360
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            #region EntityFrameworkNpgsql

            services.AddEntityFrameworkNpgsql().AddDbContext<GearContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("DataAccessPostgreSqlProvider")));

            services.AddEntityFrameworkNpgsql().AddDbContext<GearNotificationsContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("DataAccessPostgreSqlProvider")));
            
            #endregion

            #region Identity & CentralDbContext & MVC

            services.Configure<DatabaseConfigs>(Configuration.GetSection("ConnectionStrings"));

            services.AddDbContext<GearContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DataAccessPostgreSqlProvider")));


            services.AddDbContext<IGearContext, GearContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DataAccessPostgreSqlProvider")));


            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<GearContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddDefaultTokenProviders();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<MediatrResultFactory>();
                    fv.RegisterValidatorsFromAssemblyContaining<CreateProjectCommand>();
                })
                .AddJsonOptions(o => o.SerializerSettings.DateFormatString = "yyyy/MM/dd")
                .AddSessionStateTempDataProvider();

            services.AddSession();

            var azureConfig = new AzureConfig();
            Configuration.GetSection("AzureAd").Bind(azureConfig);
            services.AddAuthentication()
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = azureConfig.ClientId;
                    microsoftOptions.ClientSecret = azureConfig.ClientSecret;
                });

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory>();

            services.Configure<IndrivoNotificationOptions>(options =>
            {
                options.IndrivoAddress = "https://pm.indrivo.com";
            });
            #endregion


            #region UserInjector Configurations

            services.AddTransient<IUserAccessor, UserAccessor>();
            services.AddHttpContextAccessor();

            #endregion


            #region AutoMapper Configurations

            services.AddAutoMapper(new Assembly[] {typeof(AutoMapperProfile).GetTypeInfo().Assembly});

            var mappingConfig = new MapperConfiguration(mc =>
            {
                //Add Presentation Layer mapper Profiles here
                mc.AddProfile(new ApplicationToViewModelProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion


            #region Cloud Configurations

            services.Configure<CloudServiceSettings>(Configuration.GetSection("OneDriveSettings"));
            services.AddTransient<IStorageBaseService, OneDriveService>();
            services.AddTransient<IUserTokenDataService, BaseCloudUserDataService>();
            services.AddTransient<ICloudUserAuthorizationService, OneDriveUserAuthorizationService>();
            services.AddTransient<IHttpClientAccessor, DefaultHttpClientAccessor>();
            services.AddDbContext<ICloudStorageDb, GearContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DataAccessPostgreSqlProvider")));
            #endregion


            #region Notifications

            services.Configure<NotificationServiceSettings>(Configuration.GetSection("NotificationServiceSettings"));
            services.GearNotificationsResolver(Configuration.GetConnectionString("DataAccessPostgreSqlProvider"),
                "Gear.Notifications");

            #endregion

            #region Mail Sender

            services.AddTransient<IEmailSender, EmailSender>(i =>
                new EmailSender(
                    Configuration["EmailSender:Host"],
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    Configuration["EmailSender:UserName"],
                    Configuration["EmailSender:Password"]
                )
            );


            #endregion

            #region SSTP Configs

            services.SstpRepoResolver();
            services.AddTransient<ISstpContext, GearContext>();

            #endregion


            #region Localization Configurations

            services.AddTransient<ILocalizationService, LocalizationService>();
            services.AddTransient<IStringLocalizer, JsonStringLocalizer>();
            services.Configure<LocalizationConfigModel>(Configuration.GetSection(nameof(LocalizationConfig)));

            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencySymbol = "€";

            #endregion


            #region MediatR pipeline Configurations

            services.AddMediatR(typeof(GetProjectDetailQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(ActivateRoleCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(MediatrResultFactory).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetReportDetailsByNameQuery).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient<IMediatrResultFactory, MediatrResultFactory>();
            
            #endregion


            #region Hosted Services

            services.HostedServicesResolver();
                
            #endregion


            #region BreadCrumbs

            services.AddBreadcrumbs(GetType().Assembly, options =>
            {
                options.TagName = "nav";
                options.TagClasses = "nav-breadcrumb";
                options.OlClasses = "breadcrumb float-xl-right m-0 p-0 bg-transparent";
            });


            #endregion


            #region Coockies, Roles and Permissions

            services.AddDbContext<IPermissionContext, GearContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DataAccessPostgreSqlProvider")));

            var authCookieValidate = new AuthCookieValidate(
                new CalcAllowedPermissions(
                    services.BuildServiceProvider().GetRequiredService<IPermissionContext>()));

            services.AddTransient<ICalcAllowedPermissions, CalcAllowedPermissions>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;
                options.Events.OnValidatePrincipal = authCookieValidate.ValidateAsync;
            });

            services.AddDataProtection()
                // This helps surviving a restart: a same app will find back its keys. Just ensure to create the folder.
                .PersistKeysToFileSystem(new DirectoryInfo("\\GearPm\\keys\\"))
                // This helps surviving a site update: each app has its own store, building the site creates a new app
                .SetApplicationName("GearPm")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(90));



            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IOptionsSnapshot<LocalizationConfig> language)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            //Add localization
            var supportedCultures = language.Value.Languages?.Select(str => new CultureInfo(str.Identifier)).ToList();
            var opts = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            app.UseRequestLocalization(opts);


            var locMon = app.ApplicationServices.GetRequiredService<IOptionsMonitor<LocalizationConfigModel>>();
            locMon.OnChange(locConfig =>
            {
                var languages = locConfig.Languages.Select(lStr => new CultureInfo(lStr.Identifier)).ToList();
                var reqLoc = app.ApplicationServices.GetRequiredService<IOptionsSnapshot<RequestLocalizationOptions>>();

                reqLoc.Value.SupportedCultures = languages;
                reqLoc.Value.SupportedUICultures = languages;
            });


            app.UseHttpsRedirection();


            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<GearHub>("/gearHub");
            });

            app.UseStatusCodePages();
            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}