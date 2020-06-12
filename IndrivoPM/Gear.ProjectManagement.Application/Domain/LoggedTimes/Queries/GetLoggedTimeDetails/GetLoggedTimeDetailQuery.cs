using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeDetails
{
    public class GetLoggedTimeDetailQuery : IRequest<LoggedTimeDetailModel>
    {
        public Guid Id { get; set; }
    }
}
