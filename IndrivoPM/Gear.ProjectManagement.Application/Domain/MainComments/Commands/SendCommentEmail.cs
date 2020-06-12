using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands
{
    public class SendCommentEmail : MediatorNotification
    {
        public string Message { get; set; }

        public string AuthorName { get; set; }

        public class SendCommentEmailHandler : INotificationHandler<SendCommentEmail>
        {
            private readonly INotificationService _notification;

            public SendCommentEmailHandler(INotificationService notification)
            {
                _notification = notification;
            }

            public async Task Handle(SendCommentEmail notification, CancellationToken cancellationToken)
            {
                var message = notification.CreateNotificationMessage();

                message.From = notification.AuthorName;
                message.Body = $"{notification.AuthorName}, Mentioned you in {notification.Message}.";
                message.MessageRedirectAction = new MessageRedirectAction()
                {
                    Id = notification.PrimaryEntityId,
                    Controller = "Activities",
                    Action = "Details"
                };
                await _notification.SendAsync(message);
            }
        }
    }
}
