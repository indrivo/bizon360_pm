using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.DeleteProjectGroup
{
    public class DeleteProjectGroupCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
