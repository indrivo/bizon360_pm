using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkSetPriority
{
    public class BulkSetProjectPriorityCommand : IRequest
    {
        public ICollection<Guid> Projects { get; set; }

        public ActivityPriority Priority { get; set; }
    }
}
