using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Leader")]
        public Guid? DepartmentLeadId { get; set; }

        [DisplayName("Business Unit")]
        public Guid? BusinessUnitId { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        [DisplayName("Abbr.")]
        public string Abbreviation { get; set; }

    }
}
