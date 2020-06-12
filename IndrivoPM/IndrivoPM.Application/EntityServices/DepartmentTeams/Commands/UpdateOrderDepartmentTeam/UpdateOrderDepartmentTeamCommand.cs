using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.UpdateOrderDepartmentTeam
{
    public class UpdateOrderDepartmentTeamCommand : IRequest
    {
        public List<Guid> DepartmentTeamIds { get; set; }
    }
}
