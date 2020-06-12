using System;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.CreateLoggedTime
{
    public class CreateLoggedTimeCommand : IRequest<int?>
    {
        public Guid Id { get; set; }

        public float Time { get; set; }

        public Guid UserId { get; set; }

        [DisplayName("Activity")]
        public Guid ActivityId { get; set; }

        [DisplayName("Activity subtype")]
        public Guid TrackerId { get; set; }

        [DisplayName("Date of work")]
        public DateTime DateOfWork { get; set; }
    }
}
