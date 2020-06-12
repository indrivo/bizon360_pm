using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.RemoveJobPosition
{
    public class RemoveJobPositionCommand: IRequest
    {
        public Guid DepartmentTeamId { get; set; }

        public List<Guid> JobPositionIds { get; set; }
    }
}
