using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.DeleteBusinessUnit
{
    public class DeleteBusinessUnitCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
