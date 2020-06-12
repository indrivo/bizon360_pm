using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.RemoveActivitiesFromSprint
{
    public class RemoveActivitiesFromSprintCommand : IRequest
    {
        public ICollection<Guid> ActivitiesById { get; set; }
    }
}
