using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Background.LoggedTimePerProject
{
    public class LoggedTimePerProjectScopedService : IScopeBase<LoggedTimePerProjectScopedService>
    {
        private readonly IGearContext _context;

        private readonly IMediator _mediator;

        public LoggedTimePerProjectScopedService(IMediator mediator, IGearContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task ExecuteProcess()
        {
            var todayLoggedTime = GetUsersLoggedTimeFromDate(DateTime.Now);

            foreach (var item in todayLoggedTime)
            {
                if (item.Project.ProjectSettings.DailyMembersLoggedTime)
                    await SendNotification(item, "Logged time per team member for today: \n");

                // Send to notification profiles.
                await SendNotification(item, "Logged time per team member for today: \n", true);
            }

            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                // Logged time per team member for current week.
                var weeklyLoggedTime = GetUsersLoggedTimeFromDate(DateTime.Now.AddDays(-7));

                foreach (var item in weeklyLoggedTime)
                {
                    if (item.Project.ProjectSettings.WeeklyMembersLoggedTime)
                        await SendNotification(item, "Logged time per team member for this week: \n");

                    // Send to notification profiles.
                    await SendNotification(item, "Logged time per team member for this week: \n", true);
                }
            }

            var lastDayFromMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).Date;

            if (lastDayFromMonth == DateTime.Now.Date)
            {
                // Logged time per team member for current month.
                var monthlyLoggedTime =
                    GetUsersLoggedTimeFromDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

                foreach (var item in monthlyLoggedTime)
                {
                    if (item.Project.ProjectSettings.WeeklyMembersLoggedTime)
                        await SendNotification(item, "Logged time per team member for this month: \n");

                    // Send to notification profiles.
                    await SendNotification(item, "Logged time per team member for this month: \n", true);
                }
            }
        }
        /// <summary>
        /// Sends notification to project members or to notification profile.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toNotificationProfile"></param>
        /// <returns></returns>
        private async Task SendNotification(UsersLoggedTimeModel item, string msg, bool toNotificationProfile = false)
        {
            var c = string.Join(" \n ", item.UsersTime.Select(us => $"{us.FullName} : {us.Time}h").ToList());

            var sendTo = new List<string>();

            if (!toNotificationProfile)
            {
                sendTo.AddRange(item.UsersTime.Select(x => x.Email).ToList());
            }

            await _mediator.Publish(new LoggedTimePerProjectNotification
            {
                Recipients = sendTo,
                Body = msg + c,
                GroupEntityId = item.Project.Id,
                GroupEntityName = item.Project.Name
            }, CancellationToken.None);
        }

        private IQueryable<UsersLoggedTimeModel> GetUsersLoggedTimeFromDate(DateTime fromDate)
        {
            return _context.Projects.Include(x => x.ProjectManager).Distinct().Select(x => new UsersLoggedTimeModel
            {
                Project = x,
                UsersTime = x.ProjectMembers.Select(ut => new UsersTimeModel
                {
                    FullName = $"{ut.User.FirstName} {ut.User.LastName}",
                    Email = ut.User.Email,
                    Time = _context.Activities
                        .Include(a => a.LoggedTimes)
                        .Include(aa => aa.Assignees)
                        .Where(a => a.ProjectId == x.Id)
                        .Select(l => l.LoggedTimes
                            .Where(ll => ll.UserId == ut.UserId
                                         && ll.DateOfWork.Date >= fromDate.Date
                                         && ll.DateOfWork.Date <= DateTime.Now)
                            .Sum(s => s.Time)).Sum()
                }).ToList()
            });
        }
    }
}
