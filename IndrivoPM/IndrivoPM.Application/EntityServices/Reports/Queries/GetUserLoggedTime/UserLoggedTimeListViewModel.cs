using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTime
{
    public class UserLoggedTimeListViewModel
    {
        public string UserName { get; set; }

        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public ICollection<UserLoggedTimeLookupModel> LoggedTimes { get; set; }
    }
}
