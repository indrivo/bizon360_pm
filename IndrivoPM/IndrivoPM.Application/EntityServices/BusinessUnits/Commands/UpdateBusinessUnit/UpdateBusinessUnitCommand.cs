using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.UpdateBusinessUnit
{
    public class UpdateBusinessUnitCommand : IRequest
    {
        public Guid  Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //TODO:Remake same as when with create why would you need to delete an BU from the business perspective?
        [DisplayName("Status")]
        public bool Active { get; set; }

        [DisplayName("Leader")]
        public Guid? BusinessUnitLeadId { get; set; }

        public string Address { get; set; }
    }
}
