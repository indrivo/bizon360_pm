using System;
using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList
{
    public class HeadlineListViewModel
    {
        public Guid ProjectId { get; set; }
        public ICollection<HeadlineLookupModel> Headlines { get; set; }
    }
}
