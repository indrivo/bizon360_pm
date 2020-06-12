using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.UpdateOrderTrackerType
{
    public class UpdateOrderTrackerTypeCommand : IRequest
    {
        public List<Guid> TrackerTypeIds { get; set; }
    }
}
