using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities.Enums;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportDate;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportGuidList;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsByName;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportDetails;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers.ExcelTableHelpers;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Gear.ProjectManagement.Manager.Background.SendOverdueAttachments
{
    public class OverdueNotificationsForAdm : MediatorNotification
    {
        public class OverdueNotificationsForAdmHandler : INotificationHandler<OverdueNotificationsForAdm>
        {
            private readonly IGearContext _context;
            private readonly IMediator _mediator;
            private readonly INotificationService _notification;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IOptions<IndrivoNotificationOptions> _options;

            private readonly string _indrivoAddress;

            #region Private Methods

            private byte[] GetOverdueAttachment(OverdueTasksFilteredReportViewModel request)
            {
                var tableHelper = new ExcelTableHelper();
                var tableHeaders = tableHelper.GetOverdueTasksFilteredReportTableHeaders();
                var excelReportHelper = new ExcelReportHelper();

                return excelReportHelper.GetOverdueTasksFilteredReport(request, tableHeaders);
            }

            private string EliminatedDuplicate(string current, string previous)
            {
                return current == previous ? string.Empty : current;
            }

            private string GetOverdueHtmlContent(OverdueTasksFilteredReportViewModel request, ApplicationUser administrator)
            {
                var result = new StringBuilder();

                result.AppendLine($"Overdue tasks report for the Administrator {administrator}\n");

                result.AppendLine("<table name=\"overdue-report\">");
                result.AppendLine("<tr>");

                var tableHelper = new ExcelTableHelper();
                var tableHeaders = tableHelper.GetOverdueTasksFilteredReportTableHeaders();

                foreach (var item in tableHeaders.Where(item => item.Name != "Project Group"))
                {
                    result.AppendLine($"<th style=\"width:{item.Width}px; alignment:{item.Alignment.ToString().ToLower()};\">{item.Name}</th>");
                }

                result.AppendLine("</tr>");

                var rowOrder = 0;

                foreach (var projectGroup in request.ProjectGroups)
                {
                    var previousProjectName = string.Empty;
                    foreach (var project in projectGroup.Projects)
                    {
                        var previousActivityName = string.Empty;
                        foreach (var activity in project.Activities)
                        {
                            foreach (var assignee in activity.Assignees)
                            {
                                result.AppendLine("<tr>");
                                result.AppendLine($"<td>{++rowOrder}</td>");

                                var projectNameReference =
                                    EliminatedDuplicate(project.ProjectName, previousProjectName) != string.Empty
                                        ? $"<a href=\"{_indrivoAddress}/Activities/{project.ProjectId}\">" +
                                          $"{EliminatedDuplicate(project.ProjectName, previousProjectName)}</a>"
                                        : $"{EliminatedDuplicate(project.ProjectName, previousProjectName)}";
                                result.AppendLine(
                                    $"<td>" +
                                    $"{projectNameReference}</td>");

                                var activityNameReference =
                                    EliminatedDuplicate(activity.ActivityName, previousActivityName) != string.Empty
                                        ? $"<a href=\"{_indrivoAddress}/Activities/Details/{activity.ActivityId}\">" +
                                          $"{EliminatedDuplicate(activity.ActivityName, previousActivityName)}</a>"
                                        : $"{EliminatedDuplicate(activity.ActivityName, previousActivityName)}";
                                result.AppendLine(
                                    $"<td>" +
                                    $"{activityNameReference}</td>");

                                var assigneeNameReference =
                                    $"<a href=\"{_indrivoAddress}/ApplicationUsers/Details/{assignee.AssigneeId}\">" +
                                    $"{assignee.AssigneeName}</a>";
                                result.AppendLine(
                                    $"<td>{assigneeNameReference}</td>");

                                var deadline = assignee.Deadline.HasValue
                                    ? $"{assignee.Deadline:dd MMMM yyyy}"
                                    : "Deadline Not Setted.";
                                result.AppendLine($"<td>{deadline}</td>");
                                result.AppendLine($"<td>{assignee.Overdue}</td>");
                                result.AppendLine("</tr>");

                                previousProjectName = project.ProjectName;
                                previousActivityName = activity.ActivityName;
                            }
                        }
                    }
                }

                result.AppendLine("</table>");

                return result.ToString();
            }
            #endregion

            public OverdueNotificationsForAdmHandler(IMediator mediator, IGearContext context,
                INotificationService notification, UserManager<ApplicationUser> userManager, IOptions<IndrivoNotificationOptions> options)
            {
                _context = context;
                _mediator = mediator;
                _notification = notification;
                _userManager = userManager;
                _options = options;
                _indrivoAddress = _options.Value.IndrivoAddress;
            }

            public async Task Handle(OverdueNotificationsForAdm notification, CancellationToken cancellationToken)
            {
                if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 9)
                {
                    try
                    {
                        var service = await _context.ServiceTimeCheckers
                            .FirstOrDefaultAsync(x => x.ServiceName == "OverdueAdm", cancellationToken);

                        if (service != null && (!service.ExecutedLastTime.HasValue
                                                || (DateTime.Now - service.ExecutedLastTime.Value).TotalHours >= 22))
                        {
                            service.ExecutedLastTime = DateTime.Now;
                            _context.ServiceTimeCheckers.Update(service);
                            await _context.SaveChangesAsync(cancellationToken);
                        }
                        else return;
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.Message);
                    }

                    #region Recepients
                    var administration = await Task.Run(async () =>
                    {
                        var owners = await _userManager.GetUsersInRoleAsync("PM Owner");
                        var admins = await _userManager.GetUsersInRoleAsync("PM Admin");

                        var activeOwners = owners?.Where(x => x.Active).ToList() ?? new List<ApplicationUser>();
                        var activeAdmins = admins?.Where(x => x.Active).ToList() ?? new List<ApplicationUser>();

                        return activeOwners.Union(activeAdmins);
                    }, cancellationToken);

                    #endregion

                    var report = await _mediator.Send(new GetReportDetailsByNameQuery()
                    {
                        Name = "Overdue"
                    }, cancellationToken);

                    var now = DateTime.Now;
                    var today = $"{now.Year}-{now.Month}-{now.Day}";

                    foreach (var administrator in administration)
                    {
                        #region Filters
                        var userReport = await _mediator.Send(new GetUserReportDetailsQuery
                        {
                            UserId = administrator.Id,
                            ReportId = report.Id
                        }, cancellationToken);

                        if (!userReport.Active) continue;

                        var employeeModel = await _mediator.Send(new GetUserReportGuidListQuery()
                        {
                            UserId = administrator.Id,
                            ReportId = report.Id,
                            FilterType = FilterType.EmployeeIds
                        }, cancellationToken);

                        var projectModel = await _mediator.Send(new GetUserReportGuidListQuery()
                        {
                            UserId = administrator.Id,
                            ReportId = report.Id,
                            FilterType = FilterType.ProjectIds
                        }, cancellationToken);

                        var dateModel = await _mediator.Send(new GetUserReportDateQuery
                        {
                            UserId = administrator.Id,
                            ReportId = report.Id,
                            FilterType = FilterType.StartDate
                        }, cancellationToken);

                        #endregion

                        #region Report
                        var request = await _mediator.Send(new GetOverdueTasksFilteredReportQuery
                        {
                            AssigneeIds = employeeModel.GuidList.Distinct().ToList(),
                            StartDate = dateModel.Date,
                            DueDate = DateTime.Now,
                            ProjectIds = projectModel.GuidList.Distinct().ToList()
                        }, cancellationToken);

                        if (request == null)
                            continue;
                        var result = GetOverdueAttachment(request);
                        if (result == null)
                            continue;
                        #endregion

                        #region Notification
                        var message = new Message
                        {
                            Subject = $"Daily Report on Overdue Tasks for {today}",
                            Body = GetOverdueHtmlContent(request, administrator),
                            To = administrator.Email,
                            Attachments = new List<Attachment>
                            {
                                new Attachment
                                {
                                    Content = result,
                                    ContentId = Guid.NewGuid().ToString(),
                                    Disposition = "attachment",
                                    Filename = $"{today}_Overdue-Tasks.xlsx",
                                    Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                                }
                            },
                            EventName = "DailyOverdueNotifications",
                            SentTime = now,
                            IsBodyHtml = true
                        };

                        try
                        {
                            await _notification.SendAsync(message);
                        }
                        catch(Exception exception)
                        {
                            Debug.WriteLine(exception.Message);
                        }
                        #endregion
                    }
                }
            }
        }
    }
}
