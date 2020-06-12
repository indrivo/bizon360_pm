using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.SetActivitiesPriority
{
    public class SetActivitiesPriorityCommand : IRequest
    {
        public ActivityPriority Priority { get; set; }

        public ICollection<Guid> ActivitiesById { get; set; }
    }
}
