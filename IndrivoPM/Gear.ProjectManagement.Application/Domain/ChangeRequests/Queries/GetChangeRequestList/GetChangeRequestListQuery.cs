using System;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestList
{
    public class GetChangeRequestListQuery : IRequest<ChangeRequestListViewModel>
    {
        public Guid ProjectId { get; set; }

        public ChangeRequestStatus Status { get; set; }
    }
}
