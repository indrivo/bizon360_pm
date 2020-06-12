using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.BulkActions.BulkRefuseRequest
{
    public class BulkRefuseRequestCommand : IRequest
    {
        public ICollection<Guid> Requests { get; set; }
    }
}
