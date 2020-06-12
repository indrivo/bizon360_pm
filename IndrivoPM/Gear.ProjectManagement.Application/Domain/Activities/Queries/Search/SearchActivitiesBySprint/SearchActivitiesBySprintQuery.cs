using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesBySprint
{
    public class SearchActivitiesBySprintQuery : IRequest<SprintSearchViewModel>
    {
        public Guid ProjectId { get; set; }

        public string SearchValue { get; set; }
    }
}
