using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.UpdateOrderDepartment
{
    public class UpdateOrderDepartmentCommand : IRequest
    {
        public List<Guid> DepartmentIds { get; set; }
    }
}
