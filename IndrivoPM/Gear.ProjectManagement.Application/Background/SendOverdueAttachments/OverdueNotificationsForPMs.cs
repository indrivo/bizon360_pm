using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers.ExcelTableHelpers;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Gear.ProjectManagement.Manager.Background.SendOverdueAttachments
{
    public class OverdueNotificationsForPMs : MediatorNotification
    {
        public class OverdueNotificationsForPMsHandler : INotificationHandler<OverdueNotificationsForPMs>
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

            private string GetOverdueHtmlContent(OverdueTasksFilteredReportViewModel request, string projectManager)
            {
                var result = new StringBuilder();

                result.AppendLine($"Overdue tasks report for the Project Manager {projectManager}\n");

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
                                    ? $"{assignee.Deadline:dddd, dd MMMM yyyy}"
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

            public OverdueNotificationsForPMsHandler(IGearContext context, IMediator mediator, INotificationService notification,
                UserManager<ApplicationUser> userManager, IOptions<IndrivoNotificationOptions> options)
            {
                _context = context;
                _mediator = mediator;
                _notification = notification;
                _userManager = userManager;
                _options = options;
                _indrivoAddress = _options.Value.IndrivoAddress;
            }

            public async Task Handle(OverdueNotificationsForPMs notification, CancellationToken cancellationToken)
            {
                if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 9)
                {
                    try
                    {
                        var service = await _context.ServiceTimeCheckers
                            .FirstOrDefaultAsync(x => x.ServiceName == "OverduePm", cancellationToken);

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

                    var administration = await Task.Run(async () =>
                    {
                        var owners = await _userManager.GetUsersInRoleAsync("PM Owner");
                        var admins = await _userManager.GetUsersInRoleAsync("PM Admin");

                        var ownerEmails = owners?.Where(x => x.Active)?.Select(x => x.Email) ?? new List<string>();
                        var adminEmails = admins?.Where(x => x.Active)?.Select(x => x.Email) ?? new List<string>();

                        return ownerEmails.Union(adminEmails);
                    }, cancellationToken);

                    var teams = await Task.Run(() =>
                    {
                        var projectManagers = _context.Projects
                            .Include(x => x.ProjectManager)
                            .Include(x => x.ProjectMembers)
                            .Where(x => x.ProjectManagerId.HasValue && x.ProjectManagerId.Value != Guid.Empty
                                && x.ProjectManager != null && x.ProjectManager.Active
                                && !administration.Contains(x.ProjectManager.Email));

                        return projectManagers
                            .OrderBy(x => x.ProjectManager.Email)
                            .GroupBy(x => x.ProjectManager.Email, TeamMembersDto.Create)
                            .ToDictionary(x => x.Key, x => x);
                    }, cancellationToken);

                    foreach (var manager in teams)
                    {
                        manager.Deconstruct(out var email, out var projectEmployeeIds);
                        var employeeIds = new List<Guid>();
                        foreach (var employeeGroup in projectEmployeeIds)
                        {
                            employeeIds.AddRange(employeeGroup.EmployeeIds);
                        }

                        var projectIds = projectEmployeeIds.Select(x => x.ProjectId);

                        OverdueTasksFilteredReportViewModel request = null;
                        try 
                        {
                            request = await _mediator.Send(new GetOverdueTasksFilteredReportQuery
                            {
                                AssigneeIds = employeeIds.Distinct().ToList(),
                                StartDate = DateTime.Now.AddMonths(-12),
                                DueDate = DateTime.Now,
                                ProjectIds = projectIds.ToList()
                            }, cancellationToken);
                        }
                        catch(Exception exception)
                        {
                            Debug.WriteLine(exception.Message);
                            continue;
                        }

                        if (request == null)
                            continue;
                        var result = GetOverdueAttachment(request);
                        if (result == null)
                            continue;

                        var now = DateTime.Now;
                        var today = $"{now.Year}-{now.Month}-{now.Day}";

                        var message = new Message
                        {
                            Subject = $"Daily Report on Overdue Tasks for {today}",
                            Body = GetOverdueHtmlContent(request, manager.Key),
                            To = email,
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
                            SentTime = now
                        };

                        try
                        {
                            await _notification.SendAsync(message);
                        }
                        catch(Exception exception)
                        {
                            Debug.WriteLine(exception.Message);
                        }
                    }
                }
            }
        }
    }
}
