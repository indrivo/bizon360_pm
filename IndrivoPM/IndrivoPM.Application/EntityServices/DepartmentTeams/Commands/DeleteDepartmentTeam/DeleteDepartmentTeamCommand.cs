using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.DeleteDepartmentTeam
{
    public class DeleteDepartmentTeamCommand : IRequest
    {
        public List<Guid> Ids { get; set; }
    }
}
