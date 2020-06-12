using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bizon360.Views.Projects
{
    public static class ProjectTabs
    {
        public static string Project => "Project";

        public static string Dashboard => "Dashboard";

        public static string Activities => "Activities";

        public static string LoggedTime => "LoggedTime";

        public static string ChangeRequests => "ChangeRequests";

        public static string Files => "Files";

        public static string ActivityTypes => "Activity Types";

        public static string Wiki => "Wiki";

        public static string Reports => "Reports";

        public static string GitLab => "GitLab";

        public static string Settings => "Settings";

        public static string ProjectNavClass(ViewContext viewContext) => PageNavClass(viewContext, Project);

        public static string ActivitiesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Activities);

        public static string LoggedTimeNavClass(ViewContext viewContext) => PageNavClass(viewContext, LoggedTime);

        public static string ChangeRequestsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangeRequests);

        public static string FilesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Files);

        public static string WikiNavClass(ViewContext viewContext) => PageNavClass(viewContext, Wiki);

        public static string ActivityTypesNavClass(ViewContext viewContext) => PageNavClass(viewContext, ActivityTypes);

        public static string ReportsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Reports);

        public static string GitLabNavClass(ViewContext viewContext) => PageNavClass(viewContext, GitLab);

        public static string SettingsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Settings);

        public static string DashboardNavClass(ViewContext viewContext) => PageNavClass(viewContext, Dashboard);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                             ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
