using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Background.ProjectDeadline
{
    public class ProjectDeadlineScopedService : IScopeBase<ProjectDeadlineScopedService>
    {
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public ProjectDeadlineScopedService(IGearContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task ExecuteProcess()
        {
            var projectsMonthAway = GetProjectsDeadlineMonthAway();
            var projectsTwoWeeksAway = GetProjectsDeadlineTwoWeeksAway();
            var projectsOverdue = GetProjectsIncludes().Where(x => x.EndDate < DateTime.Now);

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                foreach (var project in projectsMonthAway)
                {
                    await SendNotification(project, "Due date of the current project is less then one month away.", true);

                    if (project.ProjectSettings.ProjectNotificationWeeklyDeadline)
                    {
                        await SendNotification(project, "Due date of the current project is less then one month away.");
                    }
                }
            }

            foreach (var project in projectsTwoWeeksAway)
            {
                await SendNotification(project, "Due date of the current project is less then two weeks away.", true);

                if (project.ProjectSettings.ProjectNotificationDailyDeadline)
                {
                    await SendNotification(project, "Due date of the current project is less then two weeks away.");
                }
            }

            foreach (var project in projectsOverdue)
            {
                await SendNotification(project, "Deadline of the current project is overdue.", true);

                if (project.ProjectSettings.ProjectNotificationDailyOverdue)
                {
                    await SendNotification(project, "Deadline of the current project is overdue.");
                }
            }
        }

        #region Query Helpers

        private IQueryable<Project> GetProjectsIncludes()
        {
            return _context.Projects
                .Include(x => x.ProjectManager)
                .Include(x => x.ProjectMembers)
                .ThenInclude(x => x.User)
                .Include(x => x.ProjectSettings)
                .Where(x => x.Active
                            && x.Status != ProjectStatus.Canceled
                            && x.Status != ProjectStatus.Completed).AsNoTracking();
        }

        private IQueryable<Project> GetProjectsDeadlineTwoWeeksAway()
        {
            var twoWeeksAway = DateTime.Now.AddDays(14).Date;

            return GetProjectsIncludes().Where(x => x.EndDate.Value.Date <= twoWeeksAway 
                                                    && x.EndDate > DateTime.Now);
        }

        private IQueryable<Project> GetProjectsDeadlineMonthAway()
        {
            var monthAway = DateTime.Now.AddMonths(1).Date;
            var twoWeeksAway = DateTime.Now.AddDays(14).Date;

            return GetProjectsIncludes().Where(x => x.EndDate.Value.Date <= monthAway
                                                     && x.EndDate.Value.Date > twoWeeksAway);
        }

        #endregion

        /// <summary>
        /// Sends notification to project members
        /// or send notification to users that exist in notification profile.
        /// </summary>
        /// <param name="project"> Project with all includes. </param>
        /// <param name="body"> Message.</param>
        /// <param name="toNotificationProfile">Set to true if you want to send to notification profiles.</param>
        /// <returns></returns>
        private async Task SendNotification(Project project, string body, bool toNotificationProfile = false)
        {
            var users = new List<string>();

            if (!toNotificationProfile)
            {
                users = project.ProjectMembers.Where(x => x.User.Active).Select(x => x.User.UserName).ToList();

                if (!users.Contains(project.ProjectManager.Email))
                {
                    users.Add(project.ProjectManager.Email);
                }
            }

            await _mediator.Publish(new ProjectDeadlineNotification
            {
                Recipients = users,
                GroupEntityName = project.Name,
                Body = body,
                GroupEntityId = project.Id
            });
        }
    }
}
