using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetMonthlyLoggedTimeForEmployee
{
    public class TimeLogModel
    {
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public DateTime Date { get; set; }

        public float Time { get; set; }

        public float Estimated { get; set; }

        public static Expression<Func<LoggedTime, TimeLogModel>> Projection
        {
            get
            {
                return timeLog => new TimeLogModel
                {
                    Id = timeLog.Id,
                    ActivityId = timeLog.ActivityId,
                    ActivityName = timeLog.Activity.Name,
                    Date = timeLog.DateOfWork,
                    Time = timeLog.Time,
                    Estimated = timeLog.Activity.EstimatedHours ?? 0
                };
            }
        }

        public static TimeLogModel Create(LoggedTime loggedTime)
        {
            return Projection.Compile().Invoke(loggedTime);
        }
    }
}