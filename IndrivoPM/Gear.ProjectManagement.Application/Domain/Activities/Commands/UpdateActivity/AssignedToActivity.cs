using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.ProjectManagement.Manager.Background;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateActivity
{
    public class AssignedToActivity : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any activity is updated.";

        public static string GroupName { get; } = "Activity_Activity";

        public static string PlatformName { get; } = "PM";

        public class AssignedToActivityHandler : INotificationHandler<AssignedToActivity>
        {
            private readonly IHostingEnvironment _environment;
            private readonly string _indrivoAddress;
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public AssignedToActivityHandler(INotificationService notification, IGearContext context, 
                IHostingEnvironment environment, IOptions<IndrivoNotificationOptions> options)
            {
                _notification = notification;
                _context = context;
                _environment = environment;
                _indrivoAddress = options.Value.IndrivoAddress;

            }

            public async Task Handle(AssignedToActivity notification, CancellationToken cancellationToken)
            {
                if (_environment.IsProduction())
                {
                    await notification.GetUsersFromProfilesAsync(_context, _notification);

                    var message = notification.CreateNotificationMessage();

                    if (message.To != "")
                    {
                        message.From = notification.UserName;
                        message.PrimaryEntityGroupName = notification.GroupEntityName;
                        message.Body = $"You have been allocated to the \"" +
                                       $"<a href=\"{_indrivoAddress}/Activities/Details/{notification.PrimaryEntityId}\">{notification.PrimaryEntityName}\"</a> activity.";
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
