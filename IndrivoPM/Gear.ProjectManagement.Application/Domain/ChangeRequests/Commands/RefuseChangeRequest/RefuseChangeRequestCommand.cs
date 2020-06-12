using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.RefuseChangeRequest
{
    public class RefuseChangeRequestCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
    }
}
