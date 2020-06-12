using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.UpdateDepartmentTeam
{
    public class UpdateDepartmentTeamCommand: IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Status")]
        public bool Active { get; set; }

        [DisplayName("Department")]
        public Guid? DepartmentId { get; set; }

        public string Description { get; set; }

        [DisplayName("Leader")]
        public Guid? DepartmentTeamLeadId { get; set; }

        [DisplayName("Members")]
        public List<Guid?> MembersIds { get; set; }

        [DisplayName("Job Positions")]
        public List<Guid?> JobPositionIds { get; set; }

        [DisplayName("Abbr.")]
        public string Abbreviation { get; set; }
    }
}
