using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.ActivateBusinessUnit
{

    public class ActivateBusinessUnitCommand : IRequest
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }
    }
}
