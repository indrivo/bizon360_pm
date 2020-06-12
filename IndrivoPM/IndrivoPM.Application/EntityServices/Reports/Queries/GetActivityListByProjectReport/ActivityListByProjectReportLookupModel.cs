using System;
using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetActivityListByProjectReport
{
    public class ActivityListByProjectReportLookupModel
    {
        public string ActivityListName { get; set; }

        public DateTime? PlannedDate { get; set; }
        
        public DateTime? ActualDate { get; set; }

        public int? Average { get; set; }

        public List<ActivityTypeLookupModel> ActivityTypes { get; set; }
            = new List<ActivityTypeLookupModel>();
    }
}
