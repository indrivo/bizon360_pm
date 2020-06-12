using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeList
{
    public class GetLoggedTimeListQuery : IRequest<LoggedTimeListViewModel>
    {
        public Guid ActivityId { get; set; }
    }
}
