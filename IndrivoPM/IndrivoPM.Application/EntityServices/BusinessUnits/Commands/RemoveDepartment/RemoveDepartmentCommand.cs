using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.RemoveDepartment
{
    public class RemoveDepartmentCommand : IRequest
    {
        public List<Guid> DepartmentIds { get; set; }
    }
}
