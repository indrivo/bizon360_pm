using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListByAssignee
{
    public class AssigneeListViewModel
    {
        public IList<AssigneeLookupModel> Assignees { get; set; }

        public ICollection<string> OpenedCollapsesById { get; set; }
    }
}
