using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.SetActivitiesStatus
{
    public class SetActivitiesStatusCommand : IRequest
    {
        public ICollection<Guid> ActivitiesById { get; set; }

        public ActivityStatus Status { get; set; }
    }
}
