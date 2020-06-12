using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.AddRemoveUserToProject
{
    public class UserAddedRemovedToProject : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any user is added or removed from project.";

        public static string GroupName { get; } = "Project_Members";

        public static string PlatformName { get; } = "PM";

        public string Message { get; set; }

        public class UserAddedRemovedToProjectHandler : INotificationHandler<UserAddedRemovedToProject>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public UserAddedRemovedToProjectHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(UserAddedRemovedToProject notification, CancellationToken cancellationToken)
            {
                var message = notification.CreateNotificationMessage();

                message.From = notification.UserName;
                message.PrimaryEntityGroupName = notification.GroupEntityName;
                message.Body = notification.Message;
                message.MessageRedirectAction = new MessageRedirectAction
                {
                    Id = notification.PrimaryEntityId,
                    Action = "Index",
                    Controller = "Projects"
                };
                await _notification.SendAsync(message);
            }
        }


    }
}
