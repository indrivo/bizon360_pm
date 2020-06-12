using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.BulkActions.BulkDeleteRequest
{
    public class BulkDeleteRequestCommand : IRequest
    {
        public ICollection<Guid> Requests { get; set; }
    }
}
