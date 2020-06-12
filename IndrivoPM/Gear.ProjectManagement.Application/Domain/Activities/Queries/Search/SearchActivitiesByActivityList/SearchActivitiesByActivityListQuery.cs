using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByActivityList
{
    public class SearchActivitiesByActivityListQuery : IRequest<ActivityListSearchViewModel>
    {
        public Guid ProjectId { get; set; }

        public string SearchValue { get; set; }
    }
}
