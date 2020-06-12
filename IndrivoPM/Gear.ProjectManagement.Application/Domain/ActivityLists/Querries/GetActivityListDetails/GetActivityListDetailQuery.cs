using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityListDetails
{
    public class GetActivityListDetailQuery : IRequest<ActivityListDetailModel>
    {
        public Guid Id { get; set; }
    }
}
