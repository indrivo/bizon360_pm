using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.AssignBusinessUnitLeader
{
    public class AssignBusinessUnitLeaderCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Leader")]
        public Guid? BusinessUnitLeadId { get; set; }

    }
}
