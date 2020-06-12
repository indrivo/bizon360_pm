using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateActivity
{
    public class ActivityUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any activity is updated.";

        public static string GroupName { get; } = "Activity_Activity";

        public static string PlatformName { get; } = "PM";

        public string Changes { get; set; }

        public class ActivityUpdatedHandler : INotificationHandler<ActivityUpdated>
        {
            private readonly IHostingEnvironment _environment;

            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public ActivityUpdatedHandler(INotificationService notification, IGearContext context,
                IHostingEnvironment environment)
            {
                _notification = notification;
                _environment = environment;
                _context = context;
            }

            public async Task Handle(ActivityUpdated notification, CancellationToken cancellationToken)
            {
                if (_environment.IsProduction())
                {
                    await notification.GetUsersFromProfilesAsync(_context, _notification);

                    var message = notification.CreateNotificationMessage();

                    if (message.To != "")
                    {
                        message.From = notification.UserName;
                        message.PrimaryEntityGroupName = notification.GroupEntityName;
                        message.Body = $"Activity \"{notification.PrimaryEntityName}\" updated with changes: " +
                                       notification.Changes;
                        message.MessageRedirectAction = new MessageRedirectAction()
                        {
                            Id = notification.PrimaryEntityId,
                            Action = "Details",
                            Controller = "Activities"
                        };
                        await _notification.SendAsync(message);
                    }
                }
            }
        }
    }
}
