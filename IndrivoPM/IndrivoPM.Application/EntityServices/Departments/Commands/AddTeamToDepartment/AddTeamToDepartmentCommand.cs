using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.AddTeamToDepartment
{
    public class AddTeamToDepartmentCommand:IRequest
    {
        public Guid DepartmentId { get; set; }

        [DisplayName("Team")]
        public List<Guid> DepartmentTeamIds { get; set; }

        public Guid? BusinessUnitId { get; set; }
    }
}
