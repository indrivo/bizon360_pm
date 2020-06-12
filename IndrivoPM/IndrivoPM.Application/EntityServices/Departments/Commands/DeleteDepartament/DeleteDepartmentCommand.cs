using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.DeleteDepartament
{
    public class DeleteDepartmentCommand:IRequest
    {
        public List<Guid> Ids { get; set; }
    }
}
