using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.UpdateOrderJobPosition
{
    public class UpdateOrderJobPositionCommand : IRequest
    {
        public List<Guid> jobPositionIds { get; set; }
    }
}
