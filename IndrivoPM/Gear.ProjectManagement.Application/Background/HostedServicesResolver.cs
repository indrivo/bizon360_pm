using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Gear.ProjectManagement.Manager.Background.DeleteOutdatedNotifications;
using Gear.ProjectManagement.Manager.Background.PlanedTasks;
using Gear.ProjectManagement.Manager.Background.SendDaylyNotifications;
using Gear.ProjectManagement.Manager.Background.SendOverdueAttachments;
using Microsoft.Extensions.DependencyInjection;

namespace Gear.ProjectManagement.Manager.Background
{
    public static class BackgroundProcessesResolver
    {
        public static IServiceCollection HostedServicesResolver(this IServiceCollection services)
        {
            services.AddHostedService<PlanedTasksHostedService>();
            services.AddScoped<IScopeBase<PlanedTasksScopedService>, PlanedTasksScopedService>();

            services.AddHostedService<SendOverdueAttachmentsHostedService>();
            services.AddScoped<IScopeBase<SendOverdueAttachmentsScopedService>, SendOverdueAttachmentsScopedService>();

            services.AddHostedService<SendDaylyNotificationsHostedService>();
            services.AddScoped<IScopeBase<SendDaylyNotificationsScopedService>, SendDaylyNotificationsScopedService>();

            services.AddHostedService<DeleteOutdatedNotificationsHostedService>();
            services.AddScoped<IScopeBase<DeleteOutdatedNotificationsScopedService>, DeleteOutdatedNotificationsScopedService>();

            return services;
        }
    }
}
