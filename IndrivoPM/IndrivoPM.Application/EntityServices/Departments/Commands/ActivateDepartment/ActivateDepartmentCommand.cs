using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.ActivateDepartment
{
   public class ActivateDepartmentCommand: IRequest
    {
        public List<Guid> Ids { get; set; }

        public bool Active { get; set; }
    }
}
