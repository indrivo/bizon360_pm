using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.ActivateTrackerType
{
    public class ActivateTrackerTypeCommand : IRequest
    {
        public List<Guid> Ids { get; set; }

        public bool Active { get; set; }
    }
}
