using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.MoveDepartment
{
    public class MoveDepartmentCommand : IRequest
    {
        [DisplayName("Business Unit")]
        public Guid? BusinessUnitId { get; set; }

        public List<Guid> DepartmentIds { get; set; }
    }
}
