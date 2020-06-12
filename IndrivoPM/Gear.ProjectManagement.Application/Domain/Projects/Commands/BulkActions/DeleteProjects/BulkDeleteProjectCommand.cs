using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.DeleteProjects
{
    public class BulkDeleteProjectCommand : IRequest
    {
        public ICollection<Guid> Projects { get; set; }
    }
}
