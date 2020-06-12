using System;
using Gear.Domain.PmEntities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bizon360.Views.Projects
{
    public static class ProjectsIndexTabs
    {
        public static string Projects => "Projects";

        public static string Current => "Current";

        public static string OnHold => "On hold";

        public static string Canceled => "Canceled";

        public static string Completed => "Completed";

        public static string Settings => "Settings";

        public static string ProjectsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Projects);

        public static string CurrentNavClass(ViewContext viewContext) => PageNavClass(viewContext, Current);

        public static string OnHoldNavClass(ViewContext viewContext) => PageNavClass(viewContext, OnHold);

        public static string CanceledNavClass(ViewContext viewContext) => PageNavClass(viewContext, Canceled);

        public static string CompletedNavClass(ViewContext viewContext) => PageNavClass(viewContext, Completed);

        public static string SettingsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Settings);

        public static string GetCurrentPage(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.OnHold:
                    return OnHold;
                case ProjectStatus.Canceled:
                    return Canceled;
                case ProjectStatus.Completed:
                    return Completed;
                default:
                    return Projects;
            }
        }

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string ??
                             System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
