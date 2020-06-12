using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTimeByPeriod
{
    public class UserLoggedTimeLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public float Time { get; set; }

        public Guid TrackerId { get; set; }

        public string TrackerName { get; set; }

        public Guid ActivityId { get; set; }

        public Tuple<Guid, string> Project { get; set; }

        public string ActivityName { get; set; }

        public DateTime DateOfWork { get; set; }

        public static Expression<Func<LoggedTime, UserLoggedTimeLookupModel>> Projection
        {
            get
            {
                return loggedTime => new UserLoggedTimeLookupModel
                {
                    Id = loggedTime.Id,
                    Name = loggedTime.Name,
                    Time = loggedTime.Time,
                    TrackerId = loggedTime.TrackerId,
                    TrackerName = loggedTime.Tracker.Name,
                    ActivityId = loggedTime.ActivityId,
                    ActivityName = loggedTime.Activity.Name,
                    Project = loggedTime.Activity != null && loggedTime.Activity.Project != null
                        ? new Tuple<Guid, string>(loggedTime.Activity.Project.Id, loggedTime.Activity.Project.Name ?? "None")
                        : new Tuple<Guid, string>(Guid.Empty, "None"),
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
