using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.UpdateRecruitmentStage
{
    public class RecruitmentStageUpdated : MediatorNotification
    {
        public string Changes { get; set; }

        public class RecruitmentStageUpdatedHandler : INotificationHandler<RecruitmentStageUpdated>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public RecruitmentStageUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }
            /// <summary>
            /// Send notification on recruitment stage update
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(RecruitmentStageUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.Body = $"Recruitment stage \"{notification.PrimaryEntityName}\" updated by {notification.UserName} with changes: " + notification.Changes;
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Action = "Index",
                        Controller = "RecruitmentStage"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
