using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByEmployee
{
    public class EmployeeSearchViewModel
    {
        public ICollection<EmployeeWithActivitiesIncludeModel> Employees { get; set; }
    }
}
