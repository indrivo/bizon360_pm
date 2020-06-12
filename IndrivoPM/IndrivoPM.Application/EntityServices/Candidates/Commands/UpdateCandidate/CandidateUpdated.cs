using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.UpdateCandidate
{
    public class CandidateUpdated : MediatorNotification
    {
        public string Changes { get; set; }

        public class CandidateUpdatedHandler : INotificationHandler<CandidateUpdated>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public CandidateUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on candidate update
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(CandidateUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.Body = $"Candidate \"{notification.PrimaryEntityName}\" updated by {notification.UserName} with changes: " + notification.Changes;
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Action = "Index",
                        Controller = "Candidate"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
