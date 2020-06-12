using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.ActivateDepartmentTeam
{
    public class ActivateDepartmentTeamCommand : IRequest
    {
        public List<Guid> Ids { get; set; }

        public bool Active { get; set; }
    }
}
