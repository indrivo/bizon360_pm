using System;
using System.Collections.Generic;
using System.Linq;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Background.SpentTimeReport
{
    public class ProjectSpentTimeScopedService : IProjectSpentTimeProcessingService
    {
        private readonly IGearContext _context;

        private readonly IMediator _mediator;

        public ProjectSpentTimeScopedService(IGearContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        /// <summary>
        /// Logic of the Process
        /// </summary>
        public async void ExecuteProcess()
        {
            try
            {
                var projectsInfo = _context.Projects.Include(x => x.ProjectSettings)
                    .Include(x => x.ProjectManager)
                    .Include(x => x.Activities)
                    .ThenInclude(x => x.Sprint)
                    .Include(x => x.Activities)
                    .ThenInclude(x => x.LoggedTimes)
                    .ThenInclude(x => x.User);

                //Check the project settings
                foreach (var project in projectsInfo)
                {
                    //If the Activity Type is selected by day / week / month
                    if (project.ProjectSettings.DailyEmailsLoggedTimeActivityType)
                    {
                        await _mediator.Publish(new SpentTimeReportNotification()
                        {
                            ProjectName = project.Name,
                            PmEmailAddress = project.ProjectManager.Email,
                            SpentTimeReportActivityType = GetDataForActivityTypes(project, DateTime.Now.AddDays(-1))
                        });
                    }

                    if (project.ProjectSettings.WeeklyEmailsLoggedTimeActivityType &&
                        DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                    {
                        await _mediator.Publish(new SpentTimeReportNotification()
                        {
                            ProjectName = project.Name,
                            PmEmailAddress = project.ProjectManager.Email,
                            SpentTimeReportActivityType = GetDataForActivityTypes(project, DateTime.Now.AddDays(-7))
                        });
                    }

                    if (project.ProjectSettings.MonthlyEmailsLoggedTimeActivityType &&
                        DateTime.Now.Day == DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                    {
                        await _mediator.Publish(new SpentTimeReportNotification()
                        {
                            ProjectName = project.Name,
                            PmEmailAddress = project.ProjectManager.Email,
                            SpentTimeReportActivityType = GetDataForActivityTypes(project, 
                                DateTime.Now.AddDays(-DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)))
                        });
                    }

                    //If the Sprint is selected by day / week / month
                    if (project.ProjectSettings.DailyEmailsLoggedTimeSprint)
                    {
                        await _mediator.Publish(new SpentTimeReportNotification()
                        {
                            ProjectName = project.Name,
                            PmEmailAddress = project.ProjectManager.Email,
                            SpentTimeReportSprint = GetDataForSprints(project, DateTime.Now.AddDays(-1))
                        });
                    }

                    if (project.ProjectSettings.WeeklyEmailsLoggedTimeSprint &&
                        DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                    {
                        await _mediator.Publish(new SpentTimeReportNotification()
                        {
                            ProjectName = project.Name,
                            PmEmailAddress = project.ProjectManager.Email,
                            SpentTimeReportSprint = GetDataForSprints(project, DateTime.Now.AddDays(-7))
                        });
                    }

                    if (project.ProjectSettings.MonthlyEmailsLoggedTimeSprint &&
                        DateTime.Now.Day == DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                    {
                        await _mediator.Publish(new SpentTimeReportNotification()
                        {
                            ProjectName = project.Name,
                            PmEmailAddress = project.ProjectManager.Email,
                            SpentTimeReportSprint = GetDataForSprints(project,
                                DateTime.Now.AddDays(-DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)))
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new BackgroundProcessException(e.Message, this.GetType().Name);
            }
        }

        /// <summary>
        /// Get the data for a specific period of time for the project sorted by activities
        /// </summary>
        /// <returns></returns>
        private List<SpentTimeReportActivityType> GetDataForActivityTypes(Project project, DateTime timeFrame)
        {
            var result = new List<SpentTimeReportActivityType>();

            var activities = project.Activities.Where(x => x.LoggedTimes.Any(y => y.CreatedTime >= timeFrame)).ToList();

            if (activities.Any())
            {
                foreach (var user in project.ProjectMembers.Select(x => x.User))
                {
                    var data = new SpentTimeReportActivityType()
                    {
                        ApplicationUser = user
                    };
                    foreach (var activityType in activities.Where(x => x.LoggedTimes.Any(y => y.UserId == user.Id))
                        .Select(x => x.ActivityType))
                    {
                        data.UserInfo.Add(new ApplicationUserActivity()
                        {
                            ActivityTypeName = activityType.ToString(),
                            UserActivities = activities.Where(x => x.LoggedTimes.Any(y => y.UserId == user.Id)).ToList()
                        });
                    }
                    result.Add(data);
                }
            }
            return result;
        }

        /// <summary>
        /// Get data for a specific period for the project sorted by sprints
        /// </summary>
        /// <param name="project"></param>
        /// <param name="timeFrameStart"></param>
        /// <returns></returns>
        private List<SpentTimeReportSprint> GetDataForSprints(Project project, DateTime timeFrameStart)
        {
            var result = new List<SpentTimeReportSprint>();
            foreach (var sprint in project.Sprints)
            {
                //Activities logged in current sprint in current time frame
                var activities = sprint.Activities.Where(
                    x => x.LoggedTimes.Any(y => y.CreatedTime > timeFrameStart)).ToList();

                if (activities.Any())
                {
                    var users = project.ProjectMembers.Select(x => x.User);

                    var data = new SpentTimeReportSprint()
                    {
                        Sprint = sprint
                    };

                    //TODO: revise this when the list of assignees will be added
                    foreach (var user in users)
                    {
                        if (activities.Any(x => x.LoggedTimes.Any(y => y.UserId == user.Id)))
                        {
                            data.UserInfo.Add(new ApplicationUserSprint()
                            {
                                ApplicationUser = user,
                                UserActivities = activities.Where(x => x.LoggedTimes.Any(y => y.UserId == user.Id)).ToList()
                            });
                        }
                    }
                    result.Add(data);
                }
            }
            return result;
        }
    }
}
