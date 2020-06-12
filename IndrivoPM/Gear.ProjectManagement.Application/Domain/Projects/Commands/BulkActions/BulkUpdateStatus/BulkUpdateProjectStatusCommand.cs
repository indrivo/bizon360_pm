using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkUpdateStatus
{
    public class BulkUpdateProjectStatusCommand : IRequest
    {
        public ICollection<Guid> Projects { get; set; }

        public ProjectStatus Status { get; set; }
    }
}
