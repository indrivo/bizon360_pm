using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeDetails;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.UpdateLoggedTime
{
    public class UpdateLoggedTimeCommand : IRequest
    {
        public Guid Id { get; set; }

        public float Time { get; set; }

        [DisplayName("Activity")]
        public Guid ActivityId { get; set; }

        [DisplayName("Activity subtype")]
        public Guid TrackerId { get; set; }

        [DisplayName("Date of work")]
        public DateTime DateOfWork { get; set; }

        public static Expression<Func<LoggedTimeDetailModel, UpdateLoggedTimeCommand>> Projection
        {
            get
            {
                return loggedTime => new UpdateLoggedTimeCommand
                {
                    Id = loggedTime.Id,
                    Time = loggedTime.Time,
                    //UserId = loggedTime.ApplicationUserId,
                    ActivityId = loggedTime.ActivityId,
                    //ActivityName = loggedTime.ActivityName,
                    TrackerId = loggedTime.TrackerId,
                    //TrackerName = loggedTime.TrackerName,
                    DateOfWork = loggedTime.DateOfWork
                };
            }
        }

        public static UpdateLoggedTimeCommand Create(LoggedTimeDetailModel loggedTime)
        {
            return Projection.Compile().Invoke(loggedTime);
        }
    }
}
