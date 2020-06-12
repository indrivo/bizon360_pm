using System;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.UpdatePriority
{
    public class UpdatePriorityCommand : IRequest
    {
        public Guid Id { get; set; }

        public ActivityPriority Priority { get; set; }
    }
}
