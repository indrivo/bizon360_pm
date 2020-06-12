using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.DeleteJobPosition
{
    public class DeleteJobPositionCommand:IRequest
    {
        public List<Guid> Ids { get; set; }
    }
}
