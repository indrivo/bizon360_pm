using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateOrderActivities
{
    public class UpdateOrderActivitiesCommand : IRequest
    {
        public List<Guid> ActivitiesIds { get; set; }
    }
}
