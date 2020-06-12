using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.DeleteActivities
{
    public class DeleteActivitiesCommand : IRequest
    {
        public ICollection<Guid> ActivitiesById { get; set; }
    }
}
