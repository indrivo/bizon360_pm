using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.ActivateJobPosition
{
    public class ActivateJobPositionCommand: IRequest
    {
        public List<Guid> Ids { get; set; }

        public bool Active { get; set; }

    }
}
