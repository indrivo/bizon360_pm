using System;
using Gear.Common.Entities;

namespace Gear.Domain.PmEntities.Settings
{
    public class ProjectSettings : BaseModel
    {
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        #region Reports Notifications

        /// <summary>
        /// Receive daily notification on logged time per team member.
        /// </summary>
        public bool DailyMembersLoggedTime { get; set; } = true;

        /// <summary>
        /// Receive weekly notification on logged time per team member.
        /// </summary>
        public bool WeeklyMembersLoggedTime { get; set; } = true;

        /// <summary>
        /// Receive monthly notification on logged time per team member.
        /// </summary>
        public bool MonthlyMembersLoggedTime { get; set; } = true;

        #endregion

        #region Email Reports on Activity Types

        /// <summary>
        /// Receive daily emails on logged time per project team member on activity types
        /// </summary>
        public bool DailyEmailsLoggedTimeActivityType { get; set; } = false;

        /// <summary>
        /// Receive weekly emails on logged time per project team member on activity types
        /// </summary>
        public bool WeeklyEmailsLoggedTimeActivityType { get; set; } = true;

        /// <summary>
        /// Receive Monthly emails on logged time per project team member on activity types
        /// </summary>
        public bool MonthlyEmailsLoggedTimeActivityType { get; set; } = true;

        #endregion

        #region Email Reports on Sprints

        /// <summary>
        /// Receive daily emails on progress per sprint for each project team member
        /// </summary>
        public bool DailyEmailsLoggedTimeSprint { get; set; } = false;

        /// <summary>
        /// Receive weekly emails on progress per sprint for each project team member
        /// </summary>
        public bool WeeklyEmailsLoggedTimeSprint { get; set; } = true;

        /// <summary>
        /// Receive monthly emails on progress per sprint for each project team member
        /// </summary>
        public bool MonthlyEmailsLoggedTimeSprint { get; set; } = true;

        #endregion

        #region ProjectTabs

        /// <summary>
        /// Display project tab on project UI
        /// </summary>
        public bool ProjectProjectTab { get; set; } = true;

        /// <summary>
        /// Display dashboard tab on project UI
        /// </summary>
        public bool ProjectDashboardTab { get; set; } = true;

        /// <summary>
        /// Display activity types dashboard UI
        /// </summary>
        public bool ActivityTypesTab { get; set; }

        /// <summary>
        /// Display activities tab on project UI
        /// </summary>
        public bool ProjectActivitiesTab { get; set; } = true;

        /// <summary>
        /// Display Logged time tab on project UI
        /// </summary>
        public bool ProjectLoggedTimeTab { get; set; } = true;
        
        /// <summary>
        /// Display change requests tab on project UI
        /// </summary>
        public bool ProjectChangeRequestsTab { get; set; } = true;

        /// <summary>
        /// Display reports tab on project UI
        /// </summary>
        public bool ProjectReportsTab { get; set; } = true;

        /// <summary>
        /// Display invoice tab on project UI
        /// </summary>
        public bool ProjectInvoiceTab { get; set; } = true;

        /// <summary>
        /// Display Wiki tab on project UI
        /// </summary>
        public bool ProjectWikiAndFilesTab { get; set; } = true;

        #endregion

        #region Activity Tabs
        /// <summary>
        /// Display activity tab on activity UI
        /// </summary>
        public bool ActivityActivityTab { get; set; } = true;

        /// <summary>
        /// Display linked activities tab on activity UI
        /// </summary>
        public bool ActivityLinkedActivitiesTab { get; set; } = true;

        /// <summary>
        /// Display comments tab on activity UI
        /// </summary>
        public bool ActivityCommentsTab { get; set; } = true;

        /// <summary>
        /// Display history tab on activity UI
        /// </summary>
        public bool ActivityHistoryTab { get; set; } = true;

        /// <summary>
        /// Display Logged time tab on activity UI
        /// </summary>
        public bool ActivityLoggedTimeTab { get; set; } = true;

        #endregion

        #region Project Notifications

        /// <summary>
        /// Receive Notification if the project is updated, completed or archived
        /// </summary>
        public bool ProjectNotificationOnUpdateCompleteArchive { get; set; } = true;

        /// <summary>
        /// Receive weekly notification if project deadline is one month away
        /// </summary>
        public bool ProjectNotificationWeeklyDeadline { get; set; } = true;

        /// <summary>
        /// Receive daily notification if project deadline is two weeks away
        /// </summary>
        public bool ProjectNotificationDailyDeadline { get; set; } = true;

        /// <summary>
        /// Receive daily notification if project is overdue.
        /// </summary>
        public bool ProjectNotificationDailyOverdue { get; set; } = true;

        #endregion

        #region Activity Notifications

        /// <summary>
        /// Receive notification if activity is created, update, completed or deleted
        /// </summary>
        public bool ActivityNotificationOnCreateUpdateCompleteDelete { get; set; } = true;

        /// <summary>
        /// Receive weekly notification if an activity deadline is one month away
        /// </summary>
        public bool ActivityNotificationWeeklyDeadline { get; set; } = false;

        /// <summary>
        /// Receive daily notification if an activity deadline is two weeks away
        /// </summary>
        public bool ActivityNotificationDailyDeadline { get; set; } = true;

        /// <summary>
        /// Receive daily notification if an activity deadline is passed
        /// </summary>
        public bool ActivityNotificationDailyOverdue { get; set; } = true;

        #endregion

        #region Sprint Notifications

        /// <summary>
        /// Receive notification if sprint is created, update, completed or deleted
        /// </summary>
        public bool SprintNotificationOnCreateUpdateCompleteDelete { get; set; } = true;

        /// <summary>
        /// Receive weekly notification if an sprint deadline is one month away
        /// </summary>
        public bool SprintNotificationWeeklyDeadline { get; set; } = false;

        /// <summary>
        /// Receive daily notification if an sprint deadline is two weeks away
        /// </summary>
        public bool SprintNotificationDailyDeadline { get; set; } = true;

        /// <summary>
        /// Receive daily notification if an sprint deadline is passed
        /// </summary>
        public bool SprintNotificationDailyOverdue { get; set; } = false;

        #endregion

        #region Activity restrictions

        /// <summary>
        /// Assignee can modify activity name.
        /// </summary>
        public bool ActivityChangeName { get; set; } = true;

        /// <summary>
        /// Asignee can move activity to another project.
        /// </summary>
        public bool ActivityChangeProject { get; set; } = true;

        /// <summary>
        /// Asignee can change the activity status.
        /// </summary>
        public bool ActivityChangeStatus { get; set; } = true;

        /// <summary>
        /// Asignee can change the activity priority.
        /// </summary>
        public bool ActivityChangeProirity { get; set; } = true;

        /// <summary>
        /// Asignee can change the activity team.
        /// </summary>
        public bool ActivityChangeTeam { get; set; } = true;

        /// <summary>
        /// Asignee can change the estimated time.
        /// </summary>
        public bool ActivityChangeEstimatedTime { get; set; } = true;

        /// <summary>
        /// Asignee can modify activty type.
        /// </summary>
        public bool ActivityChangeActivityType { get; set; } = true;

        /// <summary>
        /// Asignee can modify activity list.
        /// </summary>
        public bool ActivityChangeActivityList { get; set; } = true;

        /// <summary>
        /// Asignee can modify activity sprint.
        /// </summary>
        public bool ActivityChangeSprint { get; set; } = true;

        /// <summary>
        /// Asignee can modify activity start date and due date.
        /// </summary>
        public bool ActivityChangeStartDueDate { get; set; } = true;

        /// <summary>
        /// Asignee can delete activity.
        /// </summary>
        public bool ActivityDelete { get; set; } = true;

        #endregion
    }
}
