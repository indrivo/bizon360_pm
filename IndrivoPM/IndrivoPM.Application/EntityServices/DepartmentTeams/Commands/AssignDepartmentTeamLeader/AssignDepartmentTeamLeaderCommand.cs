using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignDepartmentTeamLeader
{
    public class AssignDepartmentTeamLeaderCommand : IRequest
    {
        public Guid? Id { get; set; }

        [DisplayName("Leader")]
        public Guid? DepartmentTeamLeadId { get; set; }

        public Guid? DepartmentId { get; set; }

    }
}
