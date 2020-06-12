using Bizon360.Models;
using Gear.Domain.PmEntities.Enums;

namespace Bizon360.Utils
{
    public static class ProjectsViewHelper
    {
        public static string Collapsed(bool condition)
        {
            return condition ? "collapsed" : string.Empty;
        }

        public static string Show(bool condition)
        {
            return condition ? "show" : string.Empty;
        }

        public static string GetPriorityBadgeColor(ActivityPriority priority)
        {
            switch (priority)
            {
                case ActivityPriority.Critical:
                    return "badge-outline-danger";
                case ActivityPriority.High:
                    return "badge-outline-warning";
                case ActivityPriority.Medium:
                    return "badge-outline-primary";
                default:
                    return "badge-outline-info";
            }
        }
        
        public static string GetPriorityColor(ActivityPriority priority)
        {
            switch (priority)
            {
                case ActivityPriority.Critical:
                    return "danger";
                case ActivityPriority.High:
                    return "warning";
                case ActivityPriority.Medium:
                    return "primary";
                default:
                    return "info";
            }
        }

        public static string GetPriorityTextColor(ActivityPriority priority)
        {
            switch (priority)
            {
                case ActivityPriority.Critical:
                    return "color-danger";
                case ActivityPriority.High:
                    return "color-warning";
                case ActivityPriority.Medium:
                    return "color-primary";
                default:
                    return "color-info";
            }
        }

        public static string GetRequestStatusBadgeColor(ChangeRequestStatus status)
        {
            switch (status)
            {
                case ChangeRequestStatus.Unprocessed:
                    return "badge-outline-primary";
                case ChangeRequestStatus.Approved:
                    return "badge-outline-success";
                case ChangeRequestStatus.Refused:
                    return "badge-outline-danger";
                default:
                    return "badge-outline-secondary";
            }
        }

        public static string GetRequestStatusColor(ChangeRequestStatus status)
        {
            switch (status)
            {
                case ChangeRequestStatus.Unprocessed:
                    return "primary";
                case ChangeRequestStatus.Approved:
                    return "success";
                case ChangeRequestStatus.Refused:
                    return "danger";
                default:
                    return "secondary";
            }
        }

        public static string GetProgressBarColor(int percentage)
        {
            return percentage <= 100 ? "bg-success" : "bg-danger";
        }

        public static ProjectsViewByStatus GetUiEnumValue(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.OnHold:
                    return ProjectsViewByStatus.OnHold;
                case ProjectStatus.Canceled:
                    return ProjectsViewByStatus.Canceled;
                case ProjectStatus.Completed:
                    return ProjectsViewByStatus.Completed;
                default:
                    return ProjectsViewByStatus.Current;
            }
        }

        public static string GetRequestStatusString(ChangeRequestStatus status)
        {
            switch (status)
            {
                case ChangeRequestStatus.Unprocessed:
                    return "unprocessed";
                case ChangeRequestStatus.Approved:
                    return "approved";
                case ChangeRequestStatus.Refused:
                    return "refused";
                default:
                    return string.Empty;
            }
        }

        public static string GetLoggedTimeAction(string action)
        {
            return action == LoggedTimeAction.Complete.ToString() ? "completed" : "updated";
        }

        public static string GetLoggedTimeCssClass(string action)
        {
            return action == LoggedTimeAction.Complete.ToString() ? "line-through" : string.Empty;
        }

        public static string GetActivityTypeTextColor(ColorBadge color)
        {
            switch (color)
            {
                case ColorBadge.Blue: return "color-primary";
                case ColorBadge.Green: return "color-success";
                case ColorBadge.Red: return "color-danger";
                case ColorBadge.Cyan: return "color-info";
                case ColorBadge.Yellow: return "color-warning";
                case ColorBadge.Purple: return "color-purple";
                case ColorBadge.Gray: return "color-secondary";
                default:
                    return "text-info";
            }
        }
    }
}
