using System.Collections.Generic;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Background.SpentTimeReport
{
    public class SpentTimeReportSprint
    {
        public Sprint Sprint { get; set; }

        public List<ApplicationUserSprint> UserInfo { get; set; } = new List<ApplicationUserSprint>();
    }

    public class ApplicationUserSprint
    {
        public ApplicationUser ApplicationUser { get; set; }

        public List<Activity> UserActivities { get; set; }
    }

    public class SpentTimeReportActivityType
    {
        public ApplicationUser ApplicationUser { get; set; }

        public List<ApplicationUserActivity> UserInfo { get; set; } = new List<ApplicationUserActivity>();
    }

    public class ApplicationUserActivity
    {
        public string ActivityTypeName { get; set; }

        public List<Activity> UserActivities { get; set; }
    }
}
