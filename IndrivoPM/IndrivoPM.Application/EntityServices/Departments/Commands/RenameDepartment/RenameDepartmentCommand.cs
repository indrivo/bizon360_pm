using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.RenameDepartment
{
    public class RenameDepartmentCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        public Guid? BusinessUnitId { get; set; }

    }
}
