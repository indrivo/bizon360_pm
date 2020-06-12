using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.ActivateActivityType
{
    public class ActivateActivityTypeCommand : IRequest
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }
    }
}
