using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.RenameJobPosition
{
    public class RenameJobPositionCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
    }
}
