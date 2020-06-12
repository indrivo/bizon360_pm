using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.MoveDepartmentTeam
{
    public class MoveDepartmentTeamCommand : IRequest
    {
        [DisplayName("Department")]
        public Guid DepartmentId { get; set; }

        [DisplayName("Team")]
        public List<Guid> DepartmentTeamIds { get; set; }
    }
}
