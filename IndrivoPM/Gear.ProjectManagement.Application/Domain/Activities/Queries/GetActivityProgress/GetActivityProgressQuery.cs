using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityProgress
{
    public class GetActivityProgressQuery : IRequest<int>
    {
        public Guid Id { get; set; }
    }
}
