using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.DeleteSprintCommand
{
    public class DeleteSprintCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
