using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeDetails
{
    public class LoggedTimeDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public float Time { get; set; }

        public Guid TrackerId { get; set; }

        public string TrackerName { get; set; }

        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public Guid ApplicationUserId { get; set; }

        public string ApplicationUserFullName { get; set; }

        public DateTime DateOfWork { get; set; }

        public int Progress { get; set; }

        public static Expression<Func<LoggedTime, LoggedTimeDetailModel>> Projection
        {
            get
            {
                return loggedTime => new LoggedTimeDetailModel
                {
                    Id = loggedTime.Id,
                    Name = loggedTime.Name,
                    Time = loggedTime.Time,
                    TrackerId = loggedTime.TrackerId,
                    TrackerName = loggedTime.Tracker.Name,
                    ActivityId = loggedTime.ActivityId,
                    ActivityName = loggedTime.Activity.Name,
                    ApplicationUserId = loggedTime.UserId,
                    ApplicationUserFullName = !string.IsNullOrEmpty(loggedTime.User.FirstName) && !string.IsNullOrEmpty(loggedTime.User.LastName) ? $"{loggedTime.User.FirstName} {loggedTime.User.LastName}" : "Not Specified",
                    DateOfWork = loggedTime.DateOfWork,
                    Progress = loggedTime.Activity != null ? loggedTime.Activity.Progress : 0
                };
            }
        }

        public static LoggedTimeDetailModel Create(LoggedTime loggedTime)
        {
            return Projection.Compile().Invoke(loggedTime);
        }
    }
}
