using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByEmployee
{
    public class SearchActivitiesByEmployeeQuery : IRequest<EmployeeSearchViewModel>
    {
        public Guid ProjectId { get; set; }

        public string SearchValue { get; set; }
    }
}
