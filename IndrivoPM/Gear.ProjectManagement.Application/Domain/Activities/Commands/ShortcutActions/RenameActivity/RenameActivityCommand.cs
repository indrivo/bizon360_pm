using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.RenameActivity
{
    public class RenameActivityCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
