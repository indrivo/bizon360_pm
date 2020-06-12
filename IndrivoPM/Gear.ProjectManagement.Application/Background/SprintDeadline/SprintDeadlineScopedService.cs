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

namespace Gear.ProjectManagement.Manager.Background.SprintDeadline
{
    public class SprintDeadlineScopedService : IScopeBase<SprintDeadlineScopedService>
    {
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public SprintDeadlineScopedService(IGearContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task ExecuteProcess()
        {
            var weekAway = DateTime.Now.AddDays(7);
            var twoWeeksAway = DateTime.Now.AddDays(14);

            var sprintsWeekAway = GetSprintsIncludes().Where(x => x.EndDate.Date <= weekAway
                                                                  && x.EndDate.Date >= DateTime.Now.Date);

            var sprintsTwoWeeksAway = GetSprintsIncludes().Where(x => x.EndDate.Date <= twoWeeksAway
                                                                      && x.EndDate.Date > weekAway);

            var sprintsOverdue = GetSprintsIncludes().Where(x => x.EndDate < DateTime.Now);

            foreach (var sprint in sprintsWeekAway)
            {
                if (sprint.Project.ProjectSettings.SprintNotificationDailyDeadline)
                {
                    await SendNotification(sprint, MsgRemainingDays(sprint));
                }

                await SendNotification(sprint, MsgRemainingDays(sprint), true);
            }


            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                foreach (var sprint in sprintsTwoWeeksAway)
                {
                    if (sprint.Project.ProjectSettings.SprintNotificationWeeklyDeadline)
                    {
                        await SendNotification(sprint, $"Due date of the \"{sprint.Name}\" sprint is less then two weeks away.");
                    }

                    await SendNotification(sprint, $"Due date of the \"{sprint.Name}\" sprint is less then two weeks away.", true);
                }
            }

            foreach (var sprint in sprintsOverdue)
            {
                if (sprint.Project.ProjectSettings.SprintNotificationDailyOverdue)
                {
                    await SendNotification(sprint, $"Due date of the \"{sprint.Name}\" sprint is overdue.");
                }

                await SendNotification(sprint, $"Due date of the \"{sprint.Name}\" sprint is overdue.", true);
            }
        }

        private string MsgRemainingDays(Sprint sprint)
        {
            var daysAway = sprint.EndDate - DateTime.Now;

            var msg = $"Due date of the \"{sprint.Name}\" sprint is {daysAway.Days+1} days away.";

            if (daysAway.Days == 0)
            {
                msg = $"Due date of the \"{sprint.Name}\" sprint is today.";
            }

            return msg;
        }

        private IQueryable<Sprint> GetSprintsIncludes()
        {
            return _context.Sprints.Include(x => x.Project)
                .ThenInclude(x => x.ProjectMembers)
                .ThenInclude(x => x.User)
                .Include(x => x.Project)
                .ThenInclude(x => x.ProjectManager)
                .Where(x => x.Active && x.SprintStatus != SprintStatus.Completed).AsNoTracking();
        }

        /// <summary>
        /// Sends notification to assigned users and to pm of to notification profiles.
        /// </summary>
        /// <param name="sprint"></param>
        /// <param name="body"></param>
        /// <param name="justToProfiles">Set to true for sending notification to notification profiles.</param>
        /// <returns></returns>
        private async Task SendNotification(Sprint sprint, string body, bool justToProfiles = false)
        {
            var users = new List<string>();

            if (!justToProfiles)
            {
                var projectManager = sprint.Project.ProjectManager.UserName;

                users.AddRange(sprint.Project.ProjectMembers.Where(x => x.User.Active)
                    .Select(x => x.User.UserName).ToList());

                if (!string.IsNullOrEmpty(projectManager) && !users.Contains(projectManager))
                    users.Add(projectManager);
            }

            await _mediator.Publish(new SprintDeadlineNotification
            {
                Body = body,
                GroupEntityName = sprint.Project.Name,
                Recipients = users,
                GroupEntityId = sprint.ProjectId.Value
            });
        }
    }
}
