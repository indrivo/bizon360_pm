using System;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CompleteCheckItem
{
    public class CompleteCheckItemCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Tracker")]
        public Guid TrackerTypeId { get; set; }

        [DisplayName("Time")]
        public float LoggedTime { get; set; } = 0;

        [DisplayName("Date of work")]
        public DateTime DateOfWorkValue { get; set; }

        public Guid ActivityId { get; set; }
    }
}
