using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.DeleteChangeRequest
{
    public class DeleteChangeRequestCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
