using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bizon360.Views.Shared.Components.ActivityTabs
{
    public static class ActivityTabs
    {
        public static string Activity => "Activity";
        public static string LoggedTime => "Logged time";
        public static string Comments => "Comments";
        public static string ActivityNavClass(ViewContext viewContext) => NavClass(viewContext, Activity);
        public static string LoggedTimeNavClass(ViewContext viewContext) => NavClass(viewContext, LoggedTime);
        public static string CommentsNavClass(ViewContext viewContext) => NavClass(viewContext, Comments);

        private static string NavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                             ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
