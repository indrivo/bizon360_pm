using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.RenameBusinessUnit
{
    public class RenameBusinessUnitCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

    }
}
