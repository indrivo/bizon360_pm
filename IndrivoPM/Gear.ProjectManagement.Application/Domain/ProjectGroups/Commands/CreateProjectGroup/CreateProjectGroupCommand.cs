using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.CreateProjectGroup
{
    public class CreateProjectGroupCommand : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

    }
}
