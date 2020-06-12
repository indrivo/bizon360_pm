using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ProjectSstp
{
    public class ProjectSstpCommand : IRequest
    {
        public Guid ProjectId { get; set; }

        public Guid? SolutionTypeId { get; set; }

        public Guid? ServiceTypeId { get; set; }

        public Guid? ProductTypeId { get; set; }

        public Guid? TechnologyTypeId { get; set; }
    }
}
