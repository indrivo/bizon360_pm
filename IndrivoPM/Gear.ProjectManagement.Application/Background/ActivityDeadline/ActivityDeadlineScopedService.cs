using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Background.ActivityDeadline
{
    public class ActivityDeadlineScopedService : IScopeBase<ActivityDeadlineScopedService>
    {
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public ActivityDeadlineScopedService(IMediator mediator, IGearContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task ExecuteProcess()
        {
            var activitiesWeekAway = GetActivitiesDeadlineWeekAway();
            var activitiesTwoWeekAway = GetActivitiesDeadlineTwoWeeksAway();
            var activitiesOverdue = GetActivitiesIncludes().Where(x => x.DueDate < DateTime.Now).AsNoTracking();

            if (activitiesWeekAway.Any())
            {
                var allowedNotifications = activitiesWeekAway.Where(x => x.Project.ProjectSettings.ActivityNotificationDailyDeadline);

                foreach (var activity in allowedNotifications)
                {
                    await SendAssigneesNotification(activity, MsgRemainingDays(activity));
                }

                await SendProjectManagerNotification(allowedNotifications, "Due date of the following activities is less then a week away: \n");
                await SendNotificationToProfiles(activitiesOverdue, "Due date of the following activities is less then a week away: \n");
            }

            if (activitiesTwoWeekAway.Any() && DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                var allowedNotifications = activitiesTwoWeekAway.Where(x => x.Project.ProjectSettings.ActivityNotificationWeeklyDeadline);

                foreach (var activity in allowedNotifications)
                {
                    await SendAssigneesNotification(activity, $"Due date of the \"{activity.Name}\" activity is less then two weeks away.");
                }

                await SendProjectManagerNotification(allowedNotifications, "Due date of the following activities is less then two weeks away: \n");
                await SendNotificationToProfiles(activitiesOverdue, "Due date of the following activities is less then two weeks away: \n");
            }

            if (activitiesOverdue.Any())
            {
                var allowedNotifications = activitiesOverdue.Where(x => x.Project.ProjectSettings.ActivityNotificationDailyOverdue);

                foreach (var activity in allowedNotifications)
                { 
                    await SendAssigneesNotification(activity, $"Due date of the \"{activity.Name}\" activity is overdue.");
                }
                
                await SendProjectManagerNotification(allowedNotifications, "Due date of the following activities is overdue: \n");
                await SendNotificationToProfiles(activitiesOverdue, "Due date of the following activities is overdue: \n");
            }
        }
        
        #region Helpers

        private string MsgRemainingDays(Activity activity)
        {
            var daysAway = activity.DueDate.Value - DateTime.Now;

            var msg = $"Due date of the \"{activity.Name}\" activity is {daysAway.Days+1} days away.";

            if (daysAway.Days == 0)
            {
                msg = $"Due date of the \"{activity.Name}\" activity is today.";
            }

            return msg;
        }

        private IQueryable<Activity> GetActivitiesIncludes()
        {
            return _context.Activities
                .Include(x => x.Project).ThenInclude(x => x.ProjectSettings)
                .Include(x => x.Project).ThenInclude(x => x.ProjectManager)
                .Include(x => x.Assignees).ThenInclude(x => x.User)
                .Include(x => x.LoggedTimes)
                .Where(x => x.ActivityStatus != ActivityStatus.Completed
                            && x.ActivityStatus != ActivityStatus.Refused).AsNoTracking();
        }

        private IQueryable<Activity> GetActivitiesDeadlineTwoWeeksAway()
        {
            var weekAway = DateTime.Now.AddDays(7).Date;
            var twoWeeksAway = DateTime.Now.AddDays(14).Date;

            return GetActivitiesIncludes().Where(x => x.DueDate.Value.Date <= twoWeeksAway
                                                      && x.DueDate.Value.Date > weekAway).AsNoTracking();
        }

        private IQueryable<Activity> GetActivitiesDeadlineWeekAway()
        {
            var weekAway = DateTime.Now.AddDays(7).Date;

            return GetActivitiesIncludes().Where(x => x.DueDate.Value.Date <= weekAway
                            && x.DueDate.Value.Date > DateTime.Now.Date).AsNoTracking();
        }

        #endregion

        #region Notification  helpers

        private async Task SendAssigneesNotification(Activity activity, string body)
        {
            var users = activity.Assignees.Where(x => x.User.Active).Select(x => x.User.UserName).ToList();

            await _mediator.Publish(new ActivityDeadlineNotification
            {
                Recipients = users,
                GroupEntityName = activity.Project.Name,
                PrimaryEntityId = activity.Id,
                Body = body,
                RedirectToActivity = true
            }, CancellationToken.None);
        }

        private async Task SendProjectManagerNotification(IQueryable<Activity> activities, string body)
        {
            var managersAndActivities = activities.Select(x => x.Project.ProjectManager.Email).Distinct().Select(y => new
            {
                PmEmail = y,
                Projects = activities.Where(p => p.Project.ProjectManager.Email.Contains(y))
                    .Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        ProjectId = p.Project.Id
                    }).Distinct().Select(p => new
                    {
                        ProjectId = p.ProjectId,
                        ProjectName = p.ProjectName,
                        Activities = activities.Where(a => a.Project.Name.Contains(p.ProjectName))
                    })
            }).ToList();



            foreach (var managersAndActivity in managersAndActivities)
            {
                foreach (var project in managersAndActivity.Projects)
                {
                    var activityNames = project.Activities.Select(x => x.Name).ToList();

                    await _mediator.Publish(new ActivityDeadlineNotification
                    {
                        Recipients = new List<string> { managersAndActivity.PmEmail },
                        Body = body + string.Join(",\n", activityNames),
                        GroupEntityName = project.ProjectName,
                        GroupEntityId = project.ProjectId
                    });
                }
            }
        }

        private async Task SendNotificationToProfiles(IQueryable<Activity> activities, string body)
        {
            var projectAndActivities = activities.Distinct().Select(p => new
            {
                ProjectId = p.Project.Id,
                ProjectName = p.Project.Name,
                Activities = activities.Where(x => x.Project.Name.Contains(p.Project.Name)).Select(x => x.Name).ToList()
            }).ToList();

            foreach (var projectAndActivity in projectAndActivities)
            {
                await _mediator.Publish(new ActivityDeadlineNotification
                {
                    Body = body + string.Join(",\n", projectAndActivity.Activities),
                    GroupEntityName = projectAndActivity.ProjectName,
                    GroupEntityId = projectAndActivity.ProjectId

                });
            }
        }

        #endregion

    }
}
