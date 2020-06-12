using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Background.SpentTimeReport
{
    public class SpentTimeReportNotification : INotification
    {
        public string ProjectName { get; set; }

        public string PmEmailAddress { get; set; }

        public List<SpentTimeReportSprint> SpentTimeReportSprint { get; set; }

        public List<SpentTimeReportActivityType> SpentTimeReportActivityType { get; set; }


        public class SpentTimeReportNotificationHandler : INotificationHandler<SpentTimeReportNotification>
        {
            private readonly INotificationService _notification;

            public SpentTimeReportNotificationHandler(INotificationService notification)
            {
                _notification = notification;
            }

            /// <summary>
            /// Method that is called to format and send the report to the PM 
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(SpentTimeReportNotification notification, CancellationToken cancellationToken)
            {

                if (notification.SpentTimeReportActivityType != null)
                {
                    var messageBody = new StringBuilder().AppendLine($"{notification.ProjectName,50}");
                    var messageTitle = new StringBuilder();

                    foreach (var report in notification.SpentTimeReportActivityType)
                    {
                        messageBody.AppendLine($" {Environment.NewLine} Report for {report.ApplicationUser.UserName} : ");
                        foreach (var activityType in report.UserInfo)
                        {
                            messageBody.AppendLine($"{activityType.ActivityTypeName}");

                            messageBody.AppendLine($"{"Activity Name:",50} {"Est",6} {"Logged",6} {"Status",10} {"Due Date",15}");

                            foreach (var activity in activityType.UserActivities)
                            {
                                var activityEstimatedHours = activity.EstimatedHours.HasValue ? activity.EstimatedHours.ToString() : "-";
                                var activityDueDate = activity.DueDate.HasValue
                                    ? activity.DueDate.Value.ToString("dd-MMM-yyyy")
                                    : "-";

                                messageBody.AppendLine($"{activity.Name,50} " +
                                                       $"{activityEstimatedHours,6} " +
                                                       $"{activity.LoggedTimes.Sum(x => x.Time),6} " +
                                                       $"{activity.ActivityStatus,10} " +
                                                       $"{activityDueDate}");
                            }
                        }
                    }
                    await _notification.SendAsync(new Message()
                    {
                        Body = messageBody.ToString(),
                        Subject = messageTitle.ToString(),
                        To = notification.PmEmailAddress
                    });
                }

                if (notification.SpentTimeReportSprint != null)
                {
                    var messageBody = new StringBuilder().AppendLine($"{notification.ProjectName,50}");
                    var messageTitle = new StringBuilder().AppendLine("Report");

                    foreach (var report in notification.SpentTimeReportSprint)
                    {
                        messageBody.AppendLine($" {Environment.NewLine} Report for {report.Sprint.Name} : ");
                        foreach (var user in report.UserInfo)
                        {
                            messageBody.AppendLine($" {Environment.NewLine} {user.ApplicationUser.UserName}");

                            messageBody.AppendLine($"{"Activity Name:",50} {"Est",6} {"Logged",6} {"Status",10} {"Due Date",15}");

                            foreach (var activity in user.UserActivities)
                            {
                                var activityEstimatedHours = activity.EstimatedHours.HasValue ? activity.EstimatedHours.ToString() : "-";
                                var activityDueDate = activity.DueDate.HasValue
                                    ? activity.DueDate.Value.ToString("dd-MMM-yyyy")
                                    : "-";

                                messageBody.AppendLine($"{activity.Name,50} " +
                                                       $"{activityEstimatedHours,6} " +
                                                       $"{activity.LoggedTimes.Where(x => x.UserId == user.ApplicationUser.Id).Sum(x => x.Time),6} " +
                                                       $"{activity.ActivityStatus,10} " +
                                                       $"{activityDueDate}");
                            }
                        }
                    }
                    await _notification.SendAsync(new Message()
                    {
                        Body = messageBody.ToString(),
                        Subject = messageTitle.ToString(),
                        To = notification.PmEmailAddress
                    });
                }
            }
        }
    }
}
