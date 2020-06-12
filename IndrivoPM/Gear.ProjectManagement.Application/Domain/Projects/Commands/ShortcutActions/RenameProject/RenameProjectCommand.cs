using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.RenameProject
{
    public class RenameProjectCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
