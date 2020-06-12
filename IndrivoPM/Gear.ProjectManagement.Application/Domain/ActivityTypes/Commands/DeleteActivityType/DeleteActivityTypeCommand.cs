using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.DeleteActivityType
{
    public class DeleteActivityTypeCommand:IRequest
    {
        public Guid Id { get; set; }

    }
}
