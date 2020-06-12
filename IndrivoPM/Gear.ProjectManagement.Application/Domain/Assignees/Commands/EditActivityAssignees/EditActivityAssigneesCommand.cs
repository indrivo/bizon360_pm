using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Assignees.Commands.EditActivityAssignees
{
    public class EditActivityAssigneesCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Assignee")]
        public ICollection<Guid> Users { get; set; }
    }
}
