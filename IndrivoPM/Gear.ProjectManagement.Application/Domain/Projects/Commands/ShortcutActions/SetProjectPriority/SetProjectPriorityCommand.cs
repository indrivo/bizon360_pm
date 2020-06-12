using System;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.SetProjectPriority
{
    public class SetProjectPriorityCommand : IRequest
    {
        public Guid Id { get; set; }

        public ActivityPriority Priority { get; set; }
    }
}
