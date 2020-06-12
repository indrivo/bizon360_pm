using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeDetails
{
    public class GetActivityTypeDetailQuery:IRequest<ActivityTypeDetailModel>
    {
        public Guid Id { get; set; }
    }
}
