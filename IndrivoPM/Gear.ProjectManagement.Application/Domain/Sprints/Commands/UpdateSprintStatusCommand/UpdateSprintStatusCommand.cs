using System;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.UpdateSprintStatusCommand
{
    public class UpdateSprintStatusCommand : IRequest
    {
        public Guid Id { get; set; }

        public SprintStatus Status { get; set; }
    }
}
