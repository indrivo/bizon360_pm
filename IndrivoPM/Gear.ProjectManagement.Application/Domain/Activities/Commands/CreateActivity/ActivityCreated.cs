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

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.CreateActivity
{
    public class ActivityCreated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any activity is created.";

        public static string GroupName { get; } = "Activity_Activity";

        public static string PlatformName { get; } = "";

        public class ActivityCreatedHandler : INotificationHandler<ActivityCreated>
        {
            private readonly IHostingEnvironment _environment;

            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            private readonly string _indrivoAddress;

            public ActivityCreatedHandler(INotificationService notification, IGearContext context,
                IOptions<IndrivoNotificationOptions> options, IHostingEnvironment environment)
            {
                _notification = notification;
                _context = context;
                _environment = environment;
                _indrivoAddress = options.Value.IndrivoAddress;
            }

            public async Task Handle(ActivityCreated notification, CancellationToken cancellationToken)
            {
                if (_environment.IsProduction())
                {
                    await notification.GetUsersFromProfilesAsync(_context, _notification);

                    var message = notification.CreateNotificationMessage();

                    if (message.To != "")
                    {
                        message.From = notification.UserName;
                        message.PrimaryEntityGroupName = notification.GroupEntityName;
                        message.Body = $"You have been allocated to the newly created activity \"" +
                                       $"<a href=\"{_indrivoAddress}/Activities/Details/{notification.PrimaryEntityId}\">{notification.PrimaryEntityName}\"</a>.";
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
