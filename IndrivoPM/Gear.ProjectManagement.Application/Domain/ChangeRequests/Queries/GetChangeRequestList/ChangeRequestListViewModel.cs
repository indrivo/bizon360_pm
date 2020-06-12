using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestList
{
    public class ChangeRequestListViewModel
    {
        public ICollection<ChangeRequestLookupModel> ChangeRequests { get; set; }
    }
}
