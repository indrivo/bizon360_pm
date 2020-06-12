using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.RemoveFromSprint
{
    public class RemoveFromSprintCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
