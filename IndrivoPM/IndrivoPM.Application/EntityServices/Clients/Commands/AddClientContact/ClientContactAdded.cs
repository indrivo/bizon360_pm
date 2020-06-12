using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.AddClientContact
{
    public class ClientContactAdded : MediatorNotification
    {
        public class ClientContactAddedHandler : INotificationHandler<ClientContactAdded>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public ClientContactAddedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(ClientContactAdded notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.Body = $"Client Contact \"{notification.PrimaryEntityName}\" was added " +
                                   $"to {notification.GroupEntityName} by {notification.UserName}.";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Controller = "Client",
                        Action = "CompanyDetails"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
