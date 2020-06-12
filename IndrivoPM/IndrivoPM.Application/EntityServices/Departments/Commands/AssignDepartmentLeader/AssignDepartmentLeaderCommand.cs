using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.AssignDepartmentLeader
{
    public class AssignDepartmentLeaderCommand : IRequest
    {
        public Guid? Id { get; set; }

        [DisplayName("Leader")]
        public Guid? DepartmentLeadId { get; set; }

        public Guid? BusinessUnitId { get; set; }
    }
}
