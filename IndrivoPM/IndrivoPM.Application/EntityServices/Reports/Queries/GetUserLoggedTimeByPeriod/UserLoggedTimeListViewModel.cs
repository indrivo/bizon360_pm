using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTimeByPeriod
{
    public class UserLoggedTimeListViewModel
    {
        public float TotalLoggedTime { get; set; }

        public ICollection<UserLoggedTimeLookupModel> LoggedTimes { get; set; }
    }
}
