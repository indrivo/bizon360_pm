using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestDetails
{
    public class GetChangeRequestDetailsQuery : IRequest<ChangeRequestDetailModel>
    {
        public Guid Id { get; set; }
    }
}
