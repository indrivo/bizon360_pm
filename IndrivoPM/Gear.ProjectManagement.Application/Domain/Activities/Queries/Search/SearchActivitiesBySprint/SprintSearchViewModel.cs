using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesBySprint
{
    public class SprintSearchViewModel
    {
        public ICollection<SprintIncludeModel> Sprints { get; set; }
    }
}
