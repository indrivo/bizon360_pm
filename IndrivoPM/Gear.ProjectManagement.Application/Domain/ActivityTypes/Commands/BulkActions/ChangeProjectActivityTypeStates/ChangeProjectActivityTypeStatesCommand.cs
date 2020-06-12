using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.BulkActions.ChangeProjectActivityTypeStates
{
    public class ChangeProjectActivityTypeStatesCommand : IRequest
    {
        public Guid ProjectId { get; set; }

        public IList<Guid> ActivityTypeIds { get; set; } 
            = new List<Guid>();

        public bool Active { get; set; }
    }
}
