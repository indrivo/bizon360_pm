using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.UpdateChangeRequest
{
    public class ChangeRequestUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any change request is updated.";

        public static string GroupName { get; } = "Project_ChangeRequest";

        public static string PlatformName { get; } = "PM";

        public string Changes { get; set; }

        public class ChangeRequestUpdatedHandler : INotificationHandler<ChangeRequestUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public ChangeRequestUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(ChangeRequestUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Updated change request named {notification.PrimaryEntityName} " +
                                   $"from project.\n" + notification.Changes;
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.GroupEntityId,
                        Controller = "Projects",
                        Action = "Details"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
