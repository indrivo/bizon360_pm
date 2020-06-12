using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.DeleteActivity
{
    public class DeleteActivityCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
