using System;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.UpdateStatus
{
    public class UpdateStatusCommand : IRequest
    {
        public Guid Id { get; set; }

        public ActivityStatus Status { get; set; }
    }
}
