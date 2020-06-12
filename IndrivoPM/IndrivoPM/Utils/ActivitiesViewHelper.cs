using System;
using System.Security.Claims;
using Bizon360.Models;
using Gear.Domain.PmEntities.Enums;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace Bizon360.Utils
{
    public static class ActivitiesViewHelper
    {
        #region NewUI

        #region View Navigation

        public static string ListNavClass(ViewContext viewContext) => ViewNavClass(viewContext, ActivitiesView.List);

        public static string SprintNavClass(ViewContext viewContext) => ViewNavClass(viewContext, ActivitiesView.Sprint);

        public static string TeamNavClass(ViewContext viewContext) => ViewNavClass(viewContext, ActivitiesView.Team);
        public static string GridNavClass(ViewContext viewContext) => ViewNavClass(viewContext, ActivitiesView.Grid);

        private static string ViewNavClass(ViewContext viewContext, ActivitiesView view)
        {
            var currentView = (ActivitiesView)viewContext.ViewBag.CurrentView;

            return string.Equals(view.ToString(), currentView.ToString(), StringComparison.OrdinalIgnoreCase)
                ? "btn-primary"
                : "btn-outline-primary";
        }

        #endregion

        #region Parent methods

        public static string ParentGetUrl(ActivitiesView currentView)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return "/ActivityLists/GetActivityLists";
                case ActivitiesView.Sprint:
                    return "/Sprints/GetSprints";
                case ActivitiesView.Team:
                    return "/ApplicationUsers/GetProjectMembers";
                case ActivitiesView.Grid:
                    return "/Activities/GetActivities";
                default:
                    return string.Empty;
            }
        }

        public static string ParentGetErrorMessage(ActivitiesView currentView, IStringLocalizer localizer)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return localizer["error_loadActivityLists"];
                case ActivitiesView.Sprint:
                    return localizer["error_loadSprints"];
                case ActivitiesView.Team:
                    return localizer["error_loadProjectMembers"];
                default:
                    return localizer["error_loadData"];
            }
        }

        #region Parent Create

        public static bool CanCreateParent(ClaimsPrincipal user, ActivitiesView currentView)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return user.UserHasThisPermission(Permissions.ActivityListCreate);
                case ActivitiesView.Sprint:
                    return user.UserHasThisPermission(Permissions.SprintCreate);
                case ActivitiesView.Team:
                    return user.UserHasThisPermission(Permissions.ProjectUpdate);
                case ActivitiesView.Grid:
                    return user.UserHasThisPermission(Permissions.ActivityCreate);
                default:
                    return false;
            }
        }

        public static bool CanUpdateParent(ClaimsPrincipal user, ActivitiesView currentView)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return user.UserHasThisPermission(Permissions.ActivityListUpdate);
                case ActivitiesView.Sprint:
                    return user.UserHasThisPermission(Permissions.SprintUpdate);
                case ActivitiesView.Team:
                    return user.UserHasThisPermission(Permissions.ProjectUpdate);
                default:
                    return false;
            }
        }

        public static bool CanDeleteParent(ClaimsPrincipal user, ActivitiesView currentView)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return user.UserHasThisPermission(Permissions.ActivityListDelete);
                case ActivitiesView.Sprint:
                    return user.UserHasThisPermission(Permissions.SprintDelete);
                case ActivitiesView.Team:
                    return user.UserHasThisPermission(Permissions.ProjectUpdate);
                default:
                    return false;
            }
        }

        public static string ParentCreateController(ActivitiesView currentView)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return "ActivityLists";
                case ActivitiesView.Sprint:
                    return "Sprints";
                case ActivitiesView.Team:
                    return "Projects";
                default:
                    return string.Empty;
            }
        }

        public static string ParentCreateAction(ActivitiesView currentView)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return "Create";
                case ActivitiesView.Sprint:
                    return "Create";
                case ActivitiesView.Team:
                    return "EditProjectMembers";
                default:
                    return string.Empty;
            }
        }

        public static string ParentCreateButtonText(ActivitiesView currentView, IStringLocalizer localizer)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return localizer["shortCuts_addActivityList"];
                case ActivitiesView.Sprint:
                    return localizer["shortCuts_addSprint"];
                case ActivitiesView.Team:
                    return localizer["shortCuts_addTeamMember"];
                default:
                    return localizer["_error"];
            }
        }

        public static string ParentGetModalSize(ActivitiesView currentView)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return "modal-lg";
                default:
                    return "regular";
            }
        }

        public static string ParentUpdateModalTitle(ActivitiesView currentView, IStringLocalizer localizer)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return localizer["shortCuts_editActivityList"];
                case ActivitiesView.Sprint:
                    return localizer["shortCuts_editSprint"];
                case ActivitiesView.Team:
                    return localizer["shortCuts_addTeamMember"];
                default:
                    return localizer["_error"];
            }
        }

        public static string ParentDeleteModalTitle(ActivitiesView currentView, IStringLocalizer localizer)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return localizer["shortCuts_deleteActivityList"];
                case ActivitiesView.Sprint:
                    return localizer["shortCuts_deleteSprint"];
                case ActivitiesView.Team:
                    return localizer["shortCuts_removeMemberFromProject"];
                default:
                    return localizer["_error"];
            }
        }

        public static string ParentType(ActivitiesView currentView, IStringLocalizer localizer)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return localizer["types_activityList"];
                case ActivitiesView.Sprint:
                    return localizer["types_sprint"];
                case ActivitiesView.Team:
                    return localizer["types_member"];
                default:
                    return localizer["_error"];
            }
        }

        public static string ParentDeleteSuccessMessage(ActivitiesView currentView, IStringLocalizer localizer)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return localizer["messages_deleteActivityListSuccess"];
                case ActivitiesView.Sprint:
                    return localizer["messages_deleteSprintSuccess"];
                case ActivitiesView.Team:
                    return localizer["messages_removeMemberSuccess"];
                default:
                    return localizer["messages_deleteSuccess"];
            }
        }

        public static string ParentDeleteErrorMessage(ActivitiesView currentView, IStringLocalizer localizer)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return localizer["messages_deleteActivityListError"];
                case ActivitiesView.Sprint:
                    return localizer["messages_deleteSprintError"];
                case ActivitiesView.Team:
                    return localizer["messages_removeMemberError"];
                default:
                    return localizer["messages_deleteError"];
            }
        }

        public static string GetSprintBadgeColor(DateTime dueDate, bool isCompleted)
        {
            if (isCompleted)
                return "badge-outline-secondary";

            var currentDate = DateTime.Now;
            var overtime = currentDate.Subtract(dueDate).Days;

            if (overtime > 0)
                return "badge-outline-danger";

            return overtime > -14 ? "badge-outline-danger" : "badge-outline-primary";
        }

        public static string GetSprintDotBadgeColor(DateTime dueDate, bool isCompleted)
        {
            if (isCompleted)
                return string.Empty;

            var currentDate = DateTime.Now;
            var overtime = currentDate.Subtract(dueDate).Days;

            if (overtime > 0)
                return "bg-danger";

            return overtime > -14 ? "bg-danger" : "bg-primary";
        }

        #endregion

        #endregion

        #region Activity methods

        public static string NoActivitiesMessage(ActivityParentType parentType, IStringLocalizer localizer)
        {
            switch (parentType)
            {
                case ActivityParentType.ActivityList:
                    return localizer["messages_noActivitiesInList"];
                case ActivityParentType.Sprint:
                    return localizer["messages_noActivitiesInSprint"];
                case ActivityParentType.Employee:
                    return localizer["messages_noActivitiesForEmployee"];
                default:
                    return localizer["messages_noActivities"];
            }
        }

        public static string NoCurrentActivitiesMessage(ActivityParentType parentType, IStringLocalizer localizer)
        {
            switch (parentType)
            {
                case ActivityParentType.ActivityList:
                    return localizer["messages_noOngoingActivitiesInList"];
                case ActivityParentType.Sprint:
                    return localizer["messages_noOngoingActivitiesInSprint"];
                case ActivityParentType.Employee:
                    return localizer["messages_noOngoingActivitiesForEmployee"];
                default:
                    return localizer["messages_noOngoingActivities"];
            }
        }

        public static string ActivityTitleClass(ActivityStatus status)
        {
            return status == ActivityStatus.Completed ? "line-through" : string.Empty;
        }

        public static string SearchUrl(ActivitiesView currentView)
        {
            switch (currentView)
            {
                case ActivitiesView.List:
                    return "/ActivityLists/SearchActivitiesByActivityList";
                case ActivitiesView.Sprint:
                    return "/Sprints/SearchActivitiesBySprint";
                case ActivitiesView.Team:
                    return "/ApplicationUsers/SearchActivitiesByEmployee";
                case ActivitiesView.Grid:
                    return "/Activities/SearchActivityByName";
                default:
                    return "/ActivityLists/SearchActivitiesByActivityList";
            }
        }

        #endregion

        #region Checklist methods

        public static string CheckItemCompleted(bool isCompleted)
        {
            return isCompleted ? "checked" : string.Empty;
        }

        public static string CheckItemLabelClass(bool isCompleted)
        {
            return isCompleted ? "line-through color-secondary" : string.Empty;
        }

        #endregion

        #endregion




        #region Parent Create

        public static string GetCreateParentButtonText(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "Sprint";
                case ActivitiesView.Team:
                    return "Employee";
                default:
                    return "Activity List";
            }
        }

        public static string GetCreateParentModalHeader(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "Add Sprint";
                case ActivitiesView.Team:
                    return "Manage Project Team";
                default:
                    return "Add Activity List";
            }
        }

        #endregion

        #region Parent Edit

        public static string GetEditParentModalHeader(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "Edit Sprint";
                case ActivitiesView.Team:
                    return "Manage Project Members";
                default:
                    return "Rename Activity List";
            }
        }

        public static string GetEditParentActionUrl(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "/Sprints/Edit";
                case ActivitiesView.Team:
                    return "/Projects/EditProjectMembers";
                default:
                    return "/Activities/EditActivityList";
            }
        }

        #endregion

        #region Parent Delete

        public static string GetDeleteParentModalHeader(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "Delete Sprint";
                case ActivitiesView.Team:
                    return string.Empty;
                default:
                    return "Delete Activity List";
            }
        }

        public static string GetDeleteParentAction(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "Delete";
                case ActivitiesView.Team:
                    return string.Empty;
                default:
                    return "DeleteActivityList";
            }
        }

        public static string GetDeleteParentController(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "Sprints";
                case ActivitiesView.Team:
                    return string.Empty;
                default:
                    return "Activities";
            }
        }

        public static string GetDeleteParentActionUrl(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "/Sprints/Delete";
                case ActivitiesView.Team:
                    return string.Empty;
                default:
                    return "/Activities/DeleteActivityList";
            }
        }

        public static string GetDeleteParentSuccessMessage(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "Sprint has been deleted";
                case ActivitiesView.Team:
                    return string.Empty;
                default:
                    return "Activity list has been deleted";
            }
        }

        public static string GetDeleteParentErrorMessage(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return "Could not remove sprint";
                case ActivitiesView.Team:
                    return string.Empty;
                default:
                    return "Could not remove activity list";
            }
        }

        #endregion

        public static bool CanCreateActivityParent(ActivitiesView view, ClaimsPrincipal user)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return user.UserHasThisPermission(Permissions.SprintCreate);
                case ActivitiesView.Team:
                    return user.UserHasThisPermission(Permissions.ProjectUpdate);
                default:
                    return user.UserHasThisPermission(Permissions.ActivityListCreate);
            }
        }

        public static bool CanEditActivityParent(ActivitiesView view, ClaimsPrincipal user)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return user.UserHasThisPermission(Permissions.SprintUpdate);
                case ActivitiesView.List:
                    return user.UserHasThisPermission(Permissions.ActivityListUpdate);
                default:
                    return false;
            }
        }

        public static bool CanDeleteActivityParent(ActivitiesView view, ClaimsPrincipal user)
        {
            switch (view)
            {
                case ActivitiesView.Sprint:
                    return user.UserHasThisPermission(Permissions.SprintDelete);
                case ActivitiesView.List:
                    return user.UserHasThisPermission(Permissions.ActivityListDelete);
                default:
                    return false;
            }
        }

        public static string GetManageId(ActivitiesView view)
        {
            switch (view)
            {
                case ActivitiesView.Team:
                    return "#modal-edit-parent";
                default:
                    return "#modal-create-parent";
            }
        }
    }
}
