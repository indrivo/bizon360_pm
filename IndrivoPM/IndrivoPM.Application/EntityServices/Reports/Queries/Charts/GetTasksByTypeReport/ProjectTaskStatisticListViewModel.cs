using System;
using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByTypeReport
{
    public class ProjectTaskStatisticListViewModel
    {
        public Guid ProjectId { get; set; }

        public IList<ActivityTypeModel> ActivityTypes { get; set; }
    }
}
