using System.Threading.Tasks;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Gear.ProjectManagement.Manager.Background.DeleteOutdatedNotifications
{
    public class DeleteOutdatedNotificationsScopedService : IScopeBase<DeleteOutdatedNotificationsScopedService>
    {
        private readonly INotificationService _service;

        private readonly IHostingEnvironment _environment;

        public DeleteOutdatedNotificationsScopedService(INotificationService service, IHostingEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        public async Task ExecuteProcess()
        {
            if (_environment.IsProduction())
            {
                var notifications = await _service.GetAllNotifications(28);
                await _service.MarkNotificationsAsRead(notifications);
            }
        }
    }
}
