using System;
using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList
{
    public class JobPositionListViewModel
    {
        public IList<JobPositionLookupModel> JobPositions { get; set; }

        public Guid ParentId { get; set; }
    }
}
