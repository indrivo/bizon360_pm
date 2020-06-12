using System;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectSettings
{
    public class ProjectSettingsModel 
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        #region Email Reports on Activity Types

        public bool DailyEmailsLoggedTimeActivityType { get; set; } = false;

        public bool WeeklyEmailsLoggedTimeActivityType { get; set; } = true;

        public bool MonthlyEmailsLoggedTimeActivityType { get; set; } = true;

        #endregion

        #region Email Reports on Sprints

        public bool DailyEmailsLoggedTimeSprint { get; set; } = false;

        public bool WeeklyEmailsLoggedTimeSprint { get; set; } = true;

        public bool MonthlyEmailsLoggedTimeSprint { get; set; } = true;

        #endregion

        #region ProjectTabs

        public bool ProjectProjectTab { get; set; } = true;

        public bool ProjectActivitiesTab { get; set; } = true;

        public bool ProjectLoggedTimeTab { get; set; } = true;

        public bool ProjectChangeRequestsTab { get; set; } = true;

        public bool ProjectReportsTab { get; set; } = true;

        public bool ProjectInvoiceTab { get; set; } = true;

        public bool ProjectWikiAndFilesTab { get; set; } = true;


        #endregion

        #region Activity Tabs

        public bool ActivityActivityTab { get; set; } = true;

        public bool ActivityLinkedActivitiesTab { get; set; } = true;

        public bool ActivityCommentsTab { get; set; } = true;

        public bool ActivityHistoryTab { get; set; } = true;

        public bool ActivityLoggedTimeTab { get; set; } = true;

        #endregion

        #region Project Notifications

        public bool ProjectNotificationOnUpdateCompleteArchive { get; set; } = false;

        public bool ProjectNotificationWeeklyDeadline { get; set; } = true;

        public bool ProjectNotificationDailyDeadline { get; set; } = true;

        public bool ProjectNotificationDailyOverdue { get; set; } = true;

        #endregion

        #region Activity Notifications

        public bool ActivityNotificationOnCreateUpdateCompleteDelete { get; set; } = true;

        public bool ActivityNotificationWeeklyDeadline { get; set; } = false;

        public bool ActivityNotificationDailyDeadline { get; set; } = true;

        public bool ActivityNotificationDailyOverdue { get; set; } = true;

        #endregion

        #region Sprint Notifications

        public bool SprintNotificationOnCreateUpdateCompleteDelete { get; set; } = true;

        public bool SprintNotificationWeeklyDeadline { get; set; } = false;

        public bool SprintNotificationDailyDeadline { get; set; } = true;

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

        #region Reports Notifications

        public bool DailyMembersLoggedTime { get; set; } = true;

        public bool WeeklyMembersLoggedTime { get; set; } = true;

        public bool MonthlyMembersLoggedTime { get; set; } = true;

        #endregion
    }
}
