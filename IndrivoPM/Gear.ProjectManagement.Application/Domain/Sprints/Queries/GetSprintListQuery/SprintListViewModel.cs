using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintListQuery
{
    public class SprintListViewModel
    {
        public ICollection<SprintLookupModel> Sprints { get; set; }
    }
}
