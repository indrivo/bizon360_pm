using System;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.MoveToSprint
{
    public class MoveToSprintCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Sprint")]
        public Guid SprintId { get; set; }
    }
}
