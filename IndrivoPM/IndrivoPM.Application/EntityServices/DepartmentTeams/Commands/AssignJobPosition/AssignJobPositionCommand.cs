using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignJobPosition
{
    public class AssignJobPositionCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Job Position")]
        public List<Guid> JobPositionIds { get; set; }

    }
}
