using System;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.CreateTrackerType
{
    public class CreateTrackerTypeCommand: IRequest
    {
        public string Name { get; set; } = "";

        [DisplayName("Activity Type")]
        public Guid ActivityTypeId { get; set; }

        [DisplayName("Status")]
        public bool Active { get; set; } = true;
    }
}
