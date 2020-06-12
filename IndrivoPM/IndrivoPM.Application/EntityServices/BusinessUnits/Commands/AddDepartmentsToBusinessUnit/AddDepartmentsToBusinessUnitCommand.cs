using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.AddDepartmentsToBusinessUnit
{
    public class AddDepartmentsToBusinessUnitCommand : IRequest
    {
        public Guid BusinessUnitId { get; set; }

        [DisplayName("Department")]
        public List<Guid> DepartmentIds { get; set; }
    }
}
