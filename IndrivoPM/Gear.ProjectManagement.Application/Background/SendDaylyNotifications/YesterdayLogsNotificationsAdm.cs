using Gear.Domain.AppEntities;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers.ExcelTableHelpers;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities.Enums;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportActivityStatuses;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportGuidList;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsByName;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Gear.ProjectManagement.Manager.Background.SendDaylyNotifications
{
    public class YesterdayLogsNotificationsAdm : MediatorNotification
    {
        public class YesterdayLogsNotificationsAdmHandler : INotificationHandler<YesterdayLogsNotificationsAdm>
        {
            private readonly IGearContext _context;

            private readonly IMediator _mediator;

            private readonly INotificationService _notification;

            private readonly UserManager<ApplicationUser> _userManager;

            private readonly IOptions<IndrivoNotificationOptions> _options;

            private readonly string _indrivoAddress;

            #region Private Methods

            private string EliminatedDuplicate(string current, string previous)
            {
                return current == previous ? string.Empty : current;
            }

            private StringBuilder GetTeamsHtmlContent(TeamsFilteredReportListViewModel request)
            {
                var result = new StringBuilder();

                result.AppendLine("<table name=\"teams-report\">");
                result.AppendLine("<tr>");

                var tableHelper = new ExcelTableHelper();
                var tableHeaders = tableHelper.GetTeamsByFiltersReportTableHeaders();

                foreach (var item in tableHeaders.Where(item => item.Name != "Activity status" && item.Name != "Est. time (hours)"))
                {
                    result.AppendLine($"<th style=\"width:{item.Width}px; alignment:{item.Alignment.ToString().ToLower()};\">{item.Name}</th>");
                }

                var rowOrder = 0;

                result.AppendLine("</tr>");

                var previousAssigneeName = string.Empty;
                foreach (var assignee in request.Assignees)
                {
                    var previousProjectName = string.Empty;
                    var previousActivityName = string.Empty;
                    foreach (var activity in assignee.ActivityView.Activities)
                    {
                        var loggedTime = activity.LoggedTimeView.LoggedTimes[0];

                        result.AppendLine("<tr>");
                        result.AppendLine($"<td>{++rowOrder}</td>");

                        var assigneeNameReference =
                        EliminatedDuplicate(assignee.AssigneeName, previousAssigneeName) != string.Empty
                            ? $"<a href=\"{_indrivoAddress}/ApplicationUsers/Details/{assignee.AssigneeId}\">" +
                              $"{EliminatedDuplicate(assignee.AssigneeName, previousAssigneeName)}</a>"
                            : $"{EliminatedDuplicate(assignee.AssigneeName, previousAssigneeName)}";
                        result.AppendLine(
                            $"<td>{assigneeNameReference}</td>");

                        var projectNameReference =
                            EliminatedDuplicate(loggedTime.ProjectName, previousProjectName) != string.Empty
                                ? $"<a href=\"{_indrivoAddress}/Activities/{activity.ProjectId}\">" +
                                  $"{EliminatedDuplicate(loggedTime.ProjectName, previousProjectName)}</a>"
                                : $"{EliminatedDuplicate(loggedTime.ProjectName, previousProjectName)}";
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

                        result.AppendLine($"<td>{activity.LoggedTimeView.TotalLoggedTime}</td>");
                        result.AppendLine("</tr>");

                        previousAssigneeName = assignee.AssigneeName;
                        previousActivityName = activity.ActivityName;
                        previousProjectName = loggedTime.ProjectName;
                    }
                }

                result.AppendLine("</table>");

                return result;
            }

            private StringBuilder GetProjectsHtmlContent(ProjectFilteredReportListViewModel request)
            {
                var result = new StringBuilder();

                result.AppendLine("<table name=\"teams-report\">");
                result.AppendLine("<tr>");

                var tableHelper = new ExcelTableHelper();
                var tableHeaders = tableHelper.GetProjectsByFiltersReportTableHeaders();

                foreach (var item in tableHeaders.Where(item => item.Name != "Activity status" && item.Name != "Est. time (hours)" && item.Name != "Activity type"))
                {
                    result.AppendLine($"<th style=\"width:{item.Width}px; alignment:{item.Alignment.ToString().ToLower()};\">{item.Name}</th>");
                }

                var rowOrder = 0;

                result.AppendLine("</tr>");

                var previousAssigneeName = string.Empty;
                foreach (var project in request.Projects)
                {
                    var previousProjectName = string.Empty;
                    var previousActivityName = string.Empty;
                    foreach (var activity in project.ActivityView.Activities)
                    {
                        foreach (var assignee in activity.AssigneeView.Assignees)
                        {
                            var loggedTime = assignee.LoggedTimeView.LoggedTimes[0];

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
                                EliminatedDuplicate(assignee.AssigneeName, previousAssigneeName) != string.Empty
                                    ? $"<a href=\"{_indrivoAddress}/ApplicationUsers/Details/{assignee.AssigneeId}\">" +
                                      $"{EliminatedDuplicate(assignee.AssigneeName, previousAssigneeName)}</a>"
                                    : $"{EliminatedDuplicate(assignee.AssigneeName, previousAssigneeName)}";
                            result.AppendLine(
                                $"<td>{assigneeNameReference}</td>");

                            result.AppendLine($"<td>{assignee.LoggedTimeView.TotalLoggedTime}</td>");
                            result.AppendLine("</tr>");

                            previousAssigneeName = assignee.AssigneeName;
                            previousActivityName = activity.ActivityName;
                            previousProjectName = project.ProjectName;
                        }
                    }
                }

                result.AppendLine("</table>");

                return result;
            }

            #endregion

            public YesterdayLogsNotificationsAdmHandler(IMediator mediator, IGearContext context,
                INotificationService notification, UserManager<ApplicationUser> userManager, IOptions<IndrivoNotificationOptions> options)
            {
                _context = context;
                _mediator = mediator;
                _notification = notification;
                _userManager = userManager;
                _options = options;
                _indrivoAddress = _options.Value.IndrivoAddress;
            }

            public async Task Handle(YesterdayLogsNotificationsAdm notification, CancellationToken cancellationToken)
            {
                if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 10 && DateTime.Now.DayOfWeek != DayOfWeek.Monday) 
                {
                    try
                    {
                        var service = await _context.ServiceTimeCheckers
                            .FirstOrDefaultAsync(x => x.ServiceName == "YesterdayLogsAdm", cancellationToken);

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

                    var teamsReport = await _mediator.Send(new GetReportDetailsByNameQuery()
                    {
                        Name = "Teams by filters"
                    }, cancellationToken);

                    var projectsReport = await _mediator.Send(new GetReportDetailsByNameQuery()
                    {
                        Name = "Projects by filters"
                    }, cancellationToken);

                    var now = DateTime.Now;
                    var dayBeforeYesterday = now.AddDays(-2);

                    var yesterdayDate = DateTime.Now.AddDays(-1);
                    var yesterday = $"{yesterdayDate.Year}-{yesterdayDate.Month}-{yesterdayDate.Day}";
                
                    var tableHelper = new ExcelTableHelper();
                    var excelReportHelper = new ExcelReportHelper();

                    foreach (var administrator in administration)
                    {
                        var userTeamsReport = await _mediator.Send(new GetUserReportDetailsQuery
                        {
                            UserId = administrator.Id,
                            ReportId = teamsReport.Id
                        }, cancellationToken);

                        var teamsHtmlContent = new StringBuilder();

                        #region Team report

                        byte[] teamsAttachment = null;

                        if (userTeamsReport != null && userTeamsReport.Active)
                        {
                            try
                            {
                                var employeeModelForTeams = await _mediator.Send(new GetUserReportGuidListQuery
                                {
                                    UserId = administrator.Id,
                                    ReportId = teamsReport.Id,
                                    FilterType = FilterType.EmployeeIds
                                }, cancellationToken);

                                var projectModelForTeams = await _mediator.Send(new GetUserReportGuidListQuery
                                {
                                    UserId = administrator.Id,
                                    ReportId = teamsReport.Id,
                                    FilterType = FilterType.ProjectIds
                                }, cancellationToken);

                                var activityTypeForTeams = await _mediator.Send(new GetUserReportGuidListQuery
                                {
                                    UserId = administrator.Id,
                                    ReportId = teamsReport.Id,
                                    FilterType = FilterType.ActivityTypeIds
                                }, cancellationToken);

                                var activityStatusesForTeams = await _mediator.Send(
                                    new GetUserReportActivityStatusesQuery
                                    {
                                        UserId = administrator.Id,
                                        ReportId = teamsReport.Id
                                    }, cancellationToken);

                                var teamsRequest = await _mediator.Send(new GetTeamsByFiltersReportQuery
                                {
                                    ProjectIds = projectModelForTeams.GuidList,
                                    ActivityStatuses = activityStatusesForTeams.ActivityStatuses,
                                    ActivityTypeIds = activityTypeForTeams.GuidList,
                                    EmployeeIds = employeeModelForTeams.GuidList,
                                    StartDate = new DateTime(dayBeforeYesterday.Year, dayBeforeYesterday.Month,
                                        dayBeforeYesterday.Day, 23, 59, 59),
                                    DueDate = now
                                }, cancellationToken);

                                var tableHeaders = tableHelper.GetTeamsByFiltersReportTableHeaders();

                                teamsAttachment = excelReportHelper.GetTeamsByFiltersReport(teamsRequest, tableHeaders);

                                if (teamsRequest != null)
                                {
                                    teamsHtmlContent = GetTeamsHtmlContent(teamsRequest);
                                }
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception.Message);
                            }
                        }

                        #endregion

                        var projectsHtmlContent = new StringBuilder();

                        #region Projects report

                        var userProjectsReport = await _mediator.Send(new GetUserReportDetailsQuery
                        {
                            UserId = administrator.Id,
                            ReportId = projectsReport.Id
                        }, cancellationToken);

                        byte[] projectsAttachment = null;

                        if (userProjectsReport != null && userProjectsReport.Active)
                        {
                            try
                            {
                                var projectModelForProjects = await _mediator.Send(new GetUserReportGuidListQuery
                                {
                                    UserId = administrator.Id,
                                    ReportId = projectsReport.Id,
                                    FilterType = FilterType.ProjectIds
                                }, cancellationToken);

                                var activityTypeForProjects = await _mediator.Send(new GetUserReportGuidListQuery
                                {
                                    UserId = administrator.Id,
                                    ReportId = projectsReport.Id,
                                    FilterType = FilterType.ActivityTypeIds
                                }, cancellationToken);

                                var activityStatusesForProjects = await _mediator.Send(
                                    new GetUserReportActivityStatusesQuery
                                    {
                                        UserId = administrator.Id,
                                        ReportId = projectsReport.Id
                                    }, cancellationToken);

                                var projectsRequest = await _mediator.Send(new GetProjectsByFiltersReportQuery
                                {
                                    ProjectIds = projectModelForProjects.GuidList,
                                    ActivityStatuses = activityStatusesForProjects.ActivityStatuses,
                                    ActivityTypes = activityTypeForProjects.GuidList,
                                    StartDate = new DateTime(dayBeforeYesterday.Year, dayBeforeYesterday.Month,
                                        dayBeforeYesterday.Day, 23, 59, 59),
                                    DueDate = now
                                }, cancellationToken);

                                var tableHeaders = tableHelper.GetProjectsByFiltersReportTableHeaders();

                                projectsAttachment =
                                    excelReportHelper.GetProjectsByFiltersReport(projectsRequest, tableHeaders);

                                projectsHtmlContent = GetProjectsHtmlContent(projectsRequest);
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception.Message);
                            }
                        }

                        #endregion

                        var attachments = new List<Attachment>();

                        if (teamsAttachment != null) attachments.Add(new Attachment
                        {
                            Content = teamsAttachment,
                            ContentId = Guid.NewGuid().ToString(),
                            Disposition = "attachment",
                            Filename = $"TodayLogs-Employees-YMD-{now.Year}-{now.Month}-{now.Day}.xlsx",
                            Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        });

                        if (projectsAttachment != null) attachments.Add(new Attachment
                        {
                            Content = projectsAttachment,
                            ContentId = Guid.NewGuid().ToString(),
                            Disposition = "attachment",
                            Filename = $"TodayLogs-Projects-YMD-{now.Year}-{now.Month}-{now.Day}.xlsx",
                            Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        });

                        if (!attachments.Any()) continue;

                        var htmlMessageContent = $"Daily logs report on yesterday for the Administrator {administrator.Email}\n"
                                                 + teamsHtmlContent
                                                 + "\n<hr>\n"
                                                 + projectsHtmlContent;

                        var message = new Message
                        {
                            Subject = $"Daily TimeLogged Report for {yesterday}",
                            Body = htmlMessageContent,
                            To = administrator.Email,
                            Attachments = attachments,
                            EventName = "DailyLogsNotifications",
                            SentTime = now,
                            From = "notification.mail@indrivo.com"
                        };

                        try
                        {
                            await _notification.SendAsync(message);
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine(exception.Message);
                        }
                    }
                }
            }
        }
    }
}
