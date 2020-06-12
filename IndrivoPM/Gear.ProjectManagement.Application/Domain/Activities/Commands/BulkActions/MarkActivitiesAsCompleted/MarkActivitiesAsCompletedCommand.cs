using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.MarkActivitiesAsCompleted
{
    public class MarkActivitiesAsCompletedCommand : IRequest
    {
        public ICollection<Guid> ActivitiesById { get; set; }
    }
}
