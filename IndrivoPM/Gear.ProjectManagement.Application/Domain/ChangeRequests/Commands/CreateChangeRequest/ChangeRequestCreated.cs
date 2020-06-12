using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.ProjectManagement.Manager.Background;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.CreateChangeRequest
{
    public class ChangeRequestCreated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any change request is created.";

        public static string GroupName { get; } = "Project_ChangeRequest";

        public static string PlatformName { get; } = "";

        public class ChangeRequestCreatedHandler : INotificationHandler<ChangeRequestCreated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            private IHostingEnvironment _environment;

            private readonly string _indrivoAddress;

            public ChangeRequestCreatedHandler(INotificationService notification, IGearContext context,
                IOptions<IndrivoNotificationOptions> options, IHostingEnvironment environment)
            {
                _notification = notification;
                _context = context;
                _environment = environment;
                _indrivoAddress = options.Value.IndrivoAddress;
            }

            public async Task Handle(ChangeRequestCreated notification, CancellationToken cancellationToken)
            {
                if (_environment.IsProduction())
                {
                    await notification.GetUsersFromProfilesAsync(_context, _notification);

                    var message = notification.CreateNotificationMessage();

                    if (message.To != "")
                    {
                        message.From = notification.UserName;
                        message.PrimaryEntityGroupName = notification.GroupEntityName;
                        message.Body = "The change request \"" +
                                       $"<a href=\"{_indrivoAddress}/ChangeRequests/Index/{notification.GroupEntityId}\">{notification.PrimaryEntityName}\"</a>" +
                                       $" in the <a href=\"{_indrivoAddress}/Projects/Details/{notification.GroupEntityId}\">{notification.GroupEntityName}</a> project.";
                        message.MessageRedirectAction = new MessageRedirectAction()
                        {
                            Id = notification.GroupEntityId,
                            Action = "Details",
                            Controller = "Projects"
                        };
                        await _notification.SendAsync(message);
                    }
                }
            }
        }
    }
}
