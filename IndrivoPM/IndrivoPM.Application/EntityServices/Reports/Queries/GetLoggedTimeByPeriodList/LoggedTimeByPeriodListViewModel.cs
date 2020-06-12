using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetLoggedTimeByPeriodList
{
    public class LoggedTimeByPeriodListViewModel
    {
        public List<UserLoggedTimeByPeriodLookupModel> UsersLoggedEstimatedTime { get; set; }
            = new List<UserLoggedTimeByPeriodLookupModel>();

        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }
    }
}
