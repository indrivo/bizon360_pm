using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.CreateClientOrganization
{
    public class ClientCompanyCreated : MediatorNotification
    {
        public class ClientCompanyCreatedHandler : INotificationHandler<ClientCompanyCreated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public ClientCompanyCreatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(ClientCompanyCreated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.Body = $"Client Company \"{notification.PrimaryEntityName}\" was added by {notification.UserName}.";
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
