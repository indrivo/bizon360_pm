using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTime
{
    public class UserLoggedTimeLookupModel
    {
        public float Time { get; set; }

        public string TrackerName { get; set; }

        public string ProjectName { get; set; }

        public string ActivityName { get; set; }

        public DateTime DateOfWork { get; set; }

        public static Expression<Func<LoggedTime, UserLoggedTimeLookupModel>> Projection
        {
            get
            {
                return loggedTime => new UserLoggedTimeLookupModel
                {
                    Time = loggedTime.Time,
                    TrackerName = loggedTime.Tracker.Name,
                    ActivityName = loggedTime.Activity.Name,
                    ProjectName = loggedTime.Activity != null && loggedTime.Activity.Project != null
                        ? (loggedTime.Activity.Project.Name ?? "None") : "None",
                    DateOfWork = loggedTime.DateOfWork
                };
            }
        }

        public static UserLoggedTimeLookupModel Create(LoggedTime loggedTime)
        {
            return Projection.Compile().Invoke(loggedTime);
        }
    }
}
