using System;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.RenameActivityType
{
    public class RenameActivityTypeCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
