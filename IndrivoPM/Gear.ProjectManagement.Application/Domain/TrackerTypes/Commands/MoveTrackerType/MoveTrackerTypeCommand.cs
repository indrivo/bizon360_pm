using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.MoveTrackerType
{
    public class MoveTrackerTypeCommand : IRequest
    {
        [DisplayName("Activity Type")]
        public Guid ActivityTypeId { get; set; }

        public List<Guid> TrackersIds { get; set; }
    }
}
