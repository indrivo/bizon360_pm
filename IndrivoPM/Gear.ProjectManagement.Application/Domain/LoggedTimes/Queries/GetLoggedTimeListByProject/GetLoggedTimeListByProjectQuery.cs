using System;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeList;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeListByProject
{
    public class GetLoggedTimeListByProjectQuery : IRequest<LoggedTimeListViewModel>
    {
        public Guid ProjectId { get; set; }
    }
}
