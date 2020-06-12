using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Background.SprintExpirationCheck
{
    public class SprintExpirationNotification : MediatorNotification
    {
        public List<Sprint> Sprints { get; set; }

        public class SprintExpirationNotificationHandler : INotificationHandler<SprintExpirationNotification>
        {
            private readonly INotificationService _notification;

            public SprintExpirationNotificationHandler(INotificationService notification)
            {
                _notification = notification;
            }

            public async Task Handle(SprintExpirationNotification notification, CancellationToken cancellationToken)
            {
                foreach (var sprint in notification.Sprints)
                {
                    var messageBody = new StringBuilder();
                    var messageTitle = new StringBuilder();

                    messageBody.AppendLine($"{sprint.Name} " +
                                           $"expires today with {sprint.Activities.Count} " +
                                           $"unfinished tasks");

                    messageBody.AppendLine(Environment.NewLine +
                                           $"The Following tasks were not completed in time please " +
                                           $"enter the project settings and choose what do you want to do with them");

                    foreach (var activity in sprint.Activities)
                    {
                        messageBody.AppendLine(Environment.NewLine + $"{"Activity Name:",50} {"Est",6} {"Status",10} {"Due Date",15}");

                        var activityEstimatedHours = activity.EstimatedHours.HasValue ? activity.EstimatedHours.ToString() : "-";
                        var activityDueDate = activity.DueDate.HasValue
                            ? activity.DueDate.Value.ToString("dd-MMM-yyyy")
                            : "-";
                        messageBody.AppendLine($"{activity.Name,50} " +
                                               $"{activityEstimatedHours,6} " +
                                               $"{activity.ActivityStatus,10} " +
                                               $"{activityDueDate}");
                    }

                    var message = notification.CreateNotificationMessage();
                    message.Body = messageBody.ToString();
                    message.Subject = messageTitle.ToString();
                    message.To = sprint.Project.ProjectManager.Email;

                    await _notification.SendAsync(message);
                }
                
            }
        }


    }
}
