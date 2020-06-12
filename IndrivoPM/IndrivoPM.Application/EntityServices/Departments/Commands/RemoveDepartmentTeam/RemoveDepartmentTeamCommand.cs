using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.RemoveDepartmentTeam
{
    public class RemoveDepartmentTeamCommand : IRequest
    {
        public List<Guid> DepartmentTeamIds { get; set; }
    }
}
