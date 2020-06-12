using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.CreateBusinessUnit
{
    public class CreateBusinessUnitCommand: IRequest
    {
        public string  Name { get; set; }

        public string Description { get; set; }

        //TODO:Remake: why would you need to create an inactive business unit?
        [DisplayName("Status")]
        public bool Active { get; set; }

        [DisplayName("Leader")]
        public Guid? BusinessUnitLeadId { get; set; }

        public string Address { get; set; }
    }
}
