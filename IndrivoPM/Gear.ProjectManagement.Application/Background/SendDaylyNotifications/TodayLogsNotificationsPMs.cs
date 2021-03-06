﻿using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers.ExcelTableHelpers;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.ProjectManagement.Manager.Background.SendOverdueAttachments;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport;

namespace Gear.ProjectManagement.Manager.Background.SendDaylyNotifications
{
    public class TodayLogsNotificationsPMs : MediatorNotification
    {
        private const string IndrivoAddress = "https://pm.indrivo.com";

        public class TodayLogsNotificationsPMsHandler : INotificationHandler<TodayLogsNotificationsPMs>
        {
            private readonly IGearContext _context;

            private readonly IMediator _mediator;
             
            private readonly INotificationService _notification;

            private readonly UserManager<ApplicationUser> _userManager;

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

                foreach (var item in tableHeaders)
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
                            ? $"<a href=\"{IndrivoAddress}/ApplicationUsers/Details/{assignee.AssigneeId}\">" +
                              $"{EliminatedDuplicate(assignee.AssigneeName, previousAssigneeName)}</a>"
                            : $"{EliminatedDuplicate(assignee.AssigneeName, previousAssigneeName)}";
                        result.AppendLine(
                            $"<td>{assigneeNameReference}</td>");

                        var projectNameReference =
                            EliminatedDuplicate(loggedTime.ProjectName, previousProjectName) != string.Empty
                                ? $"<a href=\"{IndrivoAddress}/Activities/{activity.ProjectId}\">" +
                                  $"{EliminatedDuplicate(loggedTime.ProjectName, previousProjectName)}</a>"
                                : $"{EliminatedDuplicate(loggedTime.ProjectName, previousProjectName)}";
                        result.AppendLine(
                            $"<td>" +
                            $"{projectNameReference}</td>");

                        var activityNameReference =
                            EliminatedDuplicate(activity.ActivityName, previousActivityName) != string.Empty
                                ? $"<a href=\"{IndrivoAddress}/Activities/Details/{activity.ActivityId}\">" +
                                  $"{EliminatedDuplicate(activity.ActivityName, previousActivityName)}</a>"
                                : $"{EliminatedDuplicate(activity.ActivityName, previousActivityName)}";
                        result.AppendLine(
                            $"<td>" +
                            $"{activityNameReference}</td>");

                        result.AppendLine($"<td>{loggedTime.ActivityStatus}</td>");
                        result.AppendLine($"<td>{activity.LoggedTimeView.TotalEstimatedTime}</td>");
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

            #endregion

            public TodayLogsNotificationsPMsHandler(IMediator mediator, IGearContext context, INotificationService notification, UserManager<ApplicationUser> userManager)
            {
                _mediator = mediator;
                _context = context;
                _notification = notification;
                _userManager = userManager;
            }

            public async Task Handle(TodayLogsNotificationsPMs notification, CancellationToken cancellationToken)
            {
                if (DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 23)
                {
                    try
                    {
                        var service = await _context.ServiceTimeCheckers
                            .FirstOrDefaultAsync(x => x.ServiceName == "DailyLogsPm", cancellationToken);

                        if (service != null && (!service.ExecutedLastTime.HasValue
                                                || (DateTime.Now - service.ExecutedLastTime.Value).Hours >= 22))
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

                    var administration = (await Task.Run(async () =>
                    {
                        var owners = await _userManager.GetUsersInRoleAsync("PM Owner");
                        var admins = await _userManager.GetUsersInRoleAsync("PM Admin");

                        var ownerEmails = owners?.Where(x => x.Active)?.Select(x => x.Email) ?? new List<string>();
                        var adminEmails = admins?.Where(x => x.Active)?.Select(x => x.Email) ?? new List<string>();

                        return ownerEmails.Union(adminEmails);
                    }, cancellationToken)).ToList();

                    var teams = await Task.Run(() =>
                    {
                        var projectManagers = _context.Projects
                            .Include(x => x.ProjectManager)
                            .Include(x => x.ProjectMembers)
                            .Where(x => x.ProjectManagerId.HasValue && x.ProjectManagerId.Value != Guid.Empty
                                && x.ProjectManager != null && x.ProjectManager.Active &&
                                !administration.Contains(x.ProjectManager.Email));

                        return projectManagers
                            .OrderBy(x => x.ProjectManager.Email)
                            .GroupBy(x => x.ProjectManager.Email, TeamMembersDto.Create)
                            .ToDictionary(x => x.Key, x => x);
                    }, cancellationToken);

                    var activityTypeIds = await _context.ActivityTypes.Select(x => x.Id).ToListAsync(cancellationToken);

                    var now = DateTime.Now;
                    var yesterday = now.AddDays(-1);

                    var today = $"{now.Year}-{now.Month}-{now.Day}";

                    foreach (var manager in teams)
                    {
                        manager.Deconstruct(out var email, out var projectEmployeeIds);
                        var employeeIds = new List<Guid>();
                        foreach (var employeeGroup in projectEmployeeIds)
                        {
                            employeeIds.AddRange(employeeGroup.EmployeeIds);
                        }

                        var projectIds = projectEmployeeIds.Select(x => x.ProjectId);

                        TeamsFilteredReportListViewModel request;
                        try
                        {
                            request = await _mediator.Send(new GetTeamsByFiltersReportQuery
                            {
                                ProjectIds = projectIds.ToList(),
                                ActivityStatuses = new List<ActivityStatus> { ActivityStatus.Completed, ActivityStatus.Developed,
                                 ActivityStatus.InProgress, ActivityStatus.New, ActivityStatus.Refused, ActivityStatus.Tested },
                                ActivityTypeIds = activityTypeIds,
                                EmployeeIds = employeeIds,
                                StartDate = now.AddYears(-1),
                                //StartDate = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59),
                                DueDate = now
                            }, cancellationToken);
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine(exception.Message);
                            continue;
                        }

                        var tableHelper = new ExcelTableHelper();
                        var tableHeaders = tableHelper.GetTeamsByFiltersReportTableHeaders();

                        var excelReportHelper = new ExcelReportHelper();
                        var result = excelReportHelper.GetTeamsByFiltersReport(request, tableHeaders);

                        var message = new Message
                        {
                            Subject = $"Daily Logs Report",
                            Body = $"Daily logs report for the Project Manager {manager.Key}\n" 
                                   + GetTeamsHtmlContent(request),
                            To = email,
                            Attachments = new List<Attachment>
                        {
                            new Attachment
                            {
                                Content = result,
                                ContentId = Guid.NewGuid().ToString(),
                                Disposition = "attachment",
                                Filename = $"{today}_Employee-TimeLogged.xlsx",
                                Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            }
                        },
                            EventName = "DailyLogsNotifications",
                            SentTime = now
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
