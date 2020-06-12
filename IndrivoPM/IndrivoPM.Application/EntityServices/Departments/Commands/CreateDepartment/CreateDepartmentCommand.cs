using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.CreateDepartment
{
    public  class CreateDepartmentCommand: IRequest
    {
        public string  Name { get; set; }

        public bool Active { get; set; }

        public string Description { get; set; }

        [DisplayName("Abbr.")]
        public string Abbreviation { get; set; }

        [DisplayName("Leader")]
        public Guid? DepartmentLeadId { get; set; }

        [DisplayName("Business Unit")]
        public Guid? BusinessUnitId { get; set; }
    }
}
