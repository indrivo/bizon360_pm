using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bizon360.Views.Reports
{
    public static class ReportsIndexTabs
    {
        public static string ProjectsReport => "ProjectsReport";
        public static string GeneralReport => "GeneralReport";
        public static string EmployeesReport => "EmployeesReport";
        public static string ActivitiesReport => "TasksReport";
        public static string TeamsReport => "TeamsReport";

        public static string SprintsReport => "SprintsReport";
        public static string HistoryReport => "HistoryReport";
        public static string Settings => "Settings";
        public static string ProjectsReportNavClass(ViewContext viewContext) => PageNavClass(viewContext, ProjectsReport);
        public static object GeneralReportNavClass(ViewContext viewContext) => PageNavClass(viewContext, GeneralReport);
        public static string EmployeesReportNavClass(ViewContext viewContext) => PageNavClass(viewContext, EmployeesReport);
        public static string ActivitiesReportNavClass(ViewContext viewContext) => PageNavClass(viewContext, ActivitiesReport);
        public static string TeamsReportNavClass(ViewContext viewContext) => PageNavClass(viewContext, TeamsReport);
        public static string SprintsReportNavClass(ViewContext viewContext) => PageNavClass(viewContext, SprintsReport);

        public static string HistoryReportNavClass(ViewContext viewContext) => PageNavClass(viewContext, HistoryReport);
        public static string SettingsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Settings);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string ??
                             System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
