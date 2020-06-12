using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace Gear.ProjectManagement.Manager.Background.PlanedTasks
{
    public class PlanedTasksNotification : MediatorNotification
    {
        public List<Activity> Activities { get; set; }

        public class PlanedTasksNotificationHandler : INotificationHandler<PlanedTasksNotification>
        {
            private readonly INotificationService _notification;
            private readonly string _indrivoAddress;

            #region Private Methods
            private string GetPlannedActivitiesHtmlContent(PlanedTasksNotification notification)
            {
                var result = new StringBuilder();

                result.AppendLine("<table name=\"planned-activities-report\">");
                result.AppendLine("<tr>");

                #region Headers

                result.AppendLine("<th style=\"width:25px; alignment:center;\"></th>");
                result.AppendLine("<th style=\"width:85px; alignment:left;\">Project</th>");
                result.AppendLine("<th style=\"width:85px; alignment:left;\">Activity</th>");
                result.AppendLine("<th style=\"width:45px; alignment:center;\">Activity Status</th>");
                result.AppendLine("<th style=\"width:45px; alignment:center;\">Assignment Time</th>");
                result.AppendLine("<th style=\"width:45px; alignment:right;\">Deadline</th>");

                #endregion

                var rowOrder = 0;
                var previousProjectId = Guid.Empty;
                foreach (var item in notification.Activities)
                {
                    result.AppendLine("<tr>");
                    result.AppendLine($"<td><b>{++rowOrder}</b></td>");
                    result.AppendLine(
                        previousProjectId != item.ProjectId
                            ? $"<td><a href=\"{_indrivoAddress}/Activities/{item.ProjectId}\">{item.Project.Name}</a></td>"
                            : "<td></td>");
                    result.AppendLine($"<td><a href=\"{_indrivoAddress}/Activities/Details/{item.Id}\">{item.Name}</a></td>");
                    result.AppendLine($"<td>{item.ActivityStatus}</td>");
                    result.AppendLine(item.StartDate.HasValue ? $"<td>{item.StartDate:dd MMMM yyyy}</td>" : $"<td>-</td>");
                    result.AppendLine(item.DueDate.HasValue ? $"<td>{item.DueDate:dd MMMM yyyy}</td>" : $"<td>-</td>");
                    result.AppendLine("</tr>");
                    previousProjectId = item.ProjectId;
                }

                result.AppendLine("</table>");

                return result.ToString();
            }
            #endregion

            public PlanedTasksNotificationHandler(INotificationService notification, IOptions<IndrivoNotificationOptions> options)
            {
                _notification = notification;
                _indrivoAddress = options.Value.IndrivoAddress;
            }

            public async Task Handle(PlanedTasksNotification notification, CancellationToken cancellationToken)
            {
                var message = notification.CreateNotificationMessage();
                message.Subject = "Daily report on planned tasks";
                message.Body = GetPlannedActivitiesHtmlContent(notification);
                try
                {
                    await _notification.SendAsync(message);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
    }
}
