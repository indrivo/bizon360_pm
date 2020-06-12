using System;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.MoveToProjectGroup
{
    public class MoveToProjectGroupCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Project group")]
        public Guid ProjectGroupId { get; set; }
    }
}
