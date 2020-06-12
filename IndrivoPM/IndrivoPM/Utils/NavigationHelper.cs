using System;
using Bizon360.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.String;

namespace Bizon360.Utils
{
    public static class NavigationHelper
    {
        public static Platform Pm => Platform.Pm;

        public static Platform Crm => Platform.Crm;

        public static Platform Hrm => Platform.Hrm;

        public static Platform Adm => Platform.Adm;

        public static string PmNavClass(ViewContext viewContext) => PlatformNavClass(viewContext, Pm);

        public static string CrmNavClass(ViewContext viewContext) => PlatformNavClass(viewContext, Crm);

        public static string HrmNavClass(ViewContext viewContext) => PlatformNavClass(viewContext, Hrm);

        public static string AdmNavClass(ViewContext viewContext) => PlatformNavClass(viewContext, Adm);

        private static string PlatformNavClass(ViewContext viewContext, Platform platform)
        {
            var activePlatform = viewContext.ViewData["Platform"].ToString();

            return String.Equals(activePlatform, platform.ToString(), StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static string PlatformNavClass(ViewContext viewContext)
        {
            if (viewContext.ViewData["Platform"] != null)
            {
                return Enum.IsDefined(typeof(Platform), viewContext.ViewData["Platform"]) ? 
                    Enum.GetName(typeof(Platform), viewContext.ViewData["Platform"])?.ToLower()
                    : Empty;
            }

            return Empty;
        }

        public static string DarkThemeClass(string enabled)
        {
            return enabled == "true" ? "dark-theme" : Empty;
        }
    }
}
