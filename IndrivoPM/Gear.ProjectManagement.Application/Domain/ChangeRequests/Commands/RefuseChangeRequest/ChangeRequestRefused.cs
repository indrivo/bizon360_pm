using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.RefuseChangeRequest
{
    public class ChangeRequestRefused : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any change request is refused.";

        public static string GroupName { get; } = "Project_ChangeRequest";

        public static string PlatformName { get; } = "PM";

        public class ChangeRequestRefusedHandler : INotificationHandler<ChangeRequestRefused>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public ChangeRequestRefusedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(ChangeRequestRefused notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Change request \"{notification.PrimaryEntityName}\" is refused.";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.GroupEntityId,
                        Controller = "ChangeRequests",
                        Action = "Index"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
