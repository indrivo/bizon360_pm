using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.RenameDepartmentTeam
{
    public class RenameDepartmentTeamCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        public Guid? DepartmentId { get; set; }

    }
}
