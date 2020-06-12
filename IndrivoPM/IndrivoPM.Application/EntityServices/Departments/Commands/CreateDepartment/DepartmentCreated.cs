using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.CreateDepartment
{
    public class DepartmentCreated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any department is created.";

        public static string GroupName { get; } = "Administration_Department";

        public static string PlatformName { get; } = "ADM";

        public class DepartmentCreatedHandler : INotificationHandler<DepartmentCreated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public DepartmentCreatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(DepartmentCreated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.Body = $"Department \"{notification.PrimaryEntityName}\" was created.";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Controller = "Departments",
                        Action = "Details"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
