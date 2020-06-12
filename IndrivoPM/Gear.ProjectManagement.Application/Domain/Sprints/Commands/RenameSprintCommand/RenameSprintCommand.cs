using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.RenameSprintCommand
{
    public class RenameSprintCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
