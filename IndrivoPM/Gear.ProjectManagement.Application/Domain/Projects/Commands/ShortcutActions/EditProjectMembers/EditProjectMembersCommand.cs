using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.EditProjectMembers
{
    public class EditProjectMembersCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Team")]
        public ICollection<Guid> Users { get; set; }
    }
}
