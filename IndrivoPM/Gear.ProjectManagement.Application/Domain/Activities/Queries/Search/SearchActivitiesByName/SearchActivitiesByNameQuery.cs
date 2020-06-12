using System;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivitiesForGrid;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByName
{
    public class SearchActivitiesByNameQuery : IRequest<ActivitiesForGrid>
    {
        public Guid ProjectId { get; set; }

        public string SearchValue { get; set; }
    }
}
