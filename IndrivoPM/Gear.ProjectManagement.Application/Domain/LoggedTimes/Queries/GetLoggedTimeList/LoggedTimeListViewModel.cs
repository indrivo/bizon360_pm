using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeList
{
    public class LoggedTimeListViewModel
    {
        public ICollection<LoggedTimeLookupModel> LoggedTimes { get; set; }
    }
}
