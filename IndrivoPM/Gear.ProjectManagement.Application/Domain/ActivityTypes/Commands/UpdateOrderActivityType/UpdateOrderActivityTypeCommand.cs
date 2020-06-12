using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.UpdateOrderActivityType
{
    public class UpdateOrderActivityTypeCommand : IRequest
    {
        public List<Guid> ActivityTypeIds { get; set; }
    }
}
