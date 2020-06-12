using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkMoveToProjectGroup
{
    public class BulkMoveToProjectGroupCommand : IRequest
    {
        public ICollection<Guid> Projects { get; set; }

        [DisplayName("Project group")]
        public Guid ProjectGroupId { get; set; }
    }
}
