using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.DeleteTrackerType
{
    public class DeleteTrackerTypeCommand:IRequest
    {
        public List<Guid> Ids { get; set; }
    }
}
