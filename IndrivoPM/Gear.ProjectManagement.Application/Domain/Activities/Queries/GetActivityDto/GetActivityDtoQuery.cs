using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDto
{
    public class GetActivityDtoQuery : IRequest<ActivityDto>
    {
        public Guid Id { get; set; }
    }
}
