using System;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.UpdateProjectStatus
{
    public class UpdateProjectStatusCommand : IRequest
    {
        public Guid Id { get; set; }

        public ProjectStatus Status { get; set; }
    }
}
