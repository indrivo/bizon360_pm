using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.UpdateProjectGroup
{
    public class UpdateProjectGroupCommand : IRequest
    {
        public string Name { get; set; }

        public Guid Id { get; set; }
    }
}
