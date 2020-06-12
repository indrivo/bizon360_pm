using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.CreateDepartmentTeam
{
    public class CreateDepartmentTeamCommand : IRequest
    {
        [DisplayName("Department")]
        public Guid? DepartmentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        [DisplayName("Leader")]
        public Guid? DepartmentTeamLeadId { get; set; }

        public List<Guid?> Members { get; set; }

        [DisplayName("Job positions")]
        public List<Guid?> JobPositions { get; set; }

        [DisplayName("Abbr.")]
        public string Abbreviation { get; set; }
    }
}
