using System;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetActivityListByProjectReport
{
    public class ActivityTypeLookupModel
    {
        public Guid ActivityTypeId { get; set; }

        public string Abbreveation { get; set; }

        public int? Progress { get; set; }
    }
}
