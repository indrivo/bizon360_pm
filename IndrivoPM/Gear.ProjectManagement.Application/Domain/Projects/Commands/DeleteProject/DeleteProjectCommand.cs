using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}