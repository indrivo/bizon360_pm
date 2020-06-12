using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityOverview
{
    public class GetActivityOverviewQuery : IRequest<ActivityOverviewModel>
    {
        public Guid Id { get; set; }
    }
}
