using System;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.SetUpProject
{
    public class SetUpProjectCommand : IRequest
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        #region Email Reports on Activity Types

        [DisplayName("Receive daily emails on logged time per project team member on activity types")]
        public bool DailyEmailsLoggedTimeActivityType { get; set; } = false;

        [DisplayName("Receive weekly emails on logged time per project team member on activity types")]
        public bool WeeklyEmailsLoggedTimeActivityType { get; set; } = true;

        [DisplayName("Receive Monthly emails on logged time per project team member on activity types")]
        public bool MonthlyEmailsLoggedTimeActivityType { get; set; } = true;

        #endregion

        #region Email Reports on Sprints

        [DisplayName("Receive daily emails on progress per sprint for each project team member")]
        public bool DailyEmailsLoggedTimeSprint { get; set; } = false;

        [DisplayName("Receive weekly emails on progress per sprint for each project team member")]
        public bool WeeklyEmailsLoggedTimeSprint { get; set; } = true;

        [DisplayName("Receive monthly emails on progress per sprint for each project team member")]
        public bool MonthlyEmailsLoggedTimeSprint { get; set; } = true;

        #endregion

        #region ProjectTabs
        [DisplayName("Display project tab for project team.")]
        public bool ProjectProjectTab { get; set; } = true;

        [DisplayName("Display activities tab for project team.")]
        public bool ProjectActivitiesTab { get; set; } = true;

        [DisplayName("Display requests tab for project team.")]
        public bool ProjectChangeRequestsTab { get; set; } = true;

        [DisplayName("Display reports tab for project team.")]
        public bool ProjectReportsTab { get; set; } = true;

        [DisplayName("Display invoice tab for project team.")]
        public bool ProjectInvoiceTab { get; set; } = true;

        [DisplayName("Display logged time tab for project team.")]
        public bool ProjectLoggedTimeTab { get; set; } = true;

        [DisplayName("Display wiki & files tab for project team.")]
        public bool ProjectWikiAndFilesTab { get; set; } = true;

        #endregion

        #region Activity Tabs

        [DisplayName("Display activity tab for project team.")]
        public bool ActivityActivityTab { get; set; } = true;

        [DisplayName("Display linked activities tab for project team.")]
        public bool ActivityLinkedActivitiesTab { get; set; } = true;

        [DisplayName("Display comments tab for project team.")]
        public bool ActivityCommentsTab { get; set; } = true;

        [DisplayName("Display history tab for project team.")]
        public bool ActivityHistoryTab { get; set; } = true;

        [DisplayName("Display logged time tab for project team.")]
        public bool ActivityLoggedTimeTab { get; set; } = true;

        #endregion

        #region Project Notifications

        [DisplayName("Notify if the project is created, modified or deleted.")]
        public bool ProjectNotificationOnUpdateCompleteArchive { get; set; } = false;

        [DisplayName("Notify daily if the project deadline is two weeks away.")]
        public bool ProjectNotificationWeeklyDeadline { get; set; } = true;

        [DisplayName("Notify weekly if the project deadline is one month away.")]
        public bool ProjectNotificationDailyDeadline { get; set; } = true;

        [DisplayName("Notify daily if the project is overdue.")]
        public bool ProjectNotificationDailyOverdue { get; set; } = true;

        #endregion

        #region Activity Notifications

        [DisplayName("Notify if the activity is created, modified or deleted.")]
        public bool ActivityNotificationOnCreateUpdateCompleteDelete { get; set; } = true;

        [DisplayName("Notify weekly if the activity deadline is two weeks away.")]
        public bool ActivityNotificationWeeklyDeadline { get; set; } = false;

        [DisplayName("Notify daily if the activity deadline is one week away.")]
        public bool ActivityNotificationDailyDeadline { get; set; } = true;

        [DisplayName("Notify daily if the activity is overdue.")]
        public bool ActivityNotificationDailyOverdue { get; set; } = true;

        #endregion

        #region Sprint Notifications

        [DisplayName("Notify if the sprint is created, modified or deleted.")]
        public bool SprintNotificationOnCreateUpdateCompleteDelete { get; set; } = true;

        [DisplayName("Notify weekly if the sprint deadline is two weeks away.")]
        public bool SprintNotificationWeeklyDeadline { get; set; } = false;

        [DisplayName("Notify daily if the sprint deadline is one week away.")]
        public bool SprintNotificationDailyDeadline { get; set; } = true;

        [DisplayName("Notify daily if the sprint is overdue.")]
        public bool SprintNotificationDailyOverdue { get; set; } = false;

        #endregion

        #region Activity restrictions

        [DisplayName("Enable name change. Assignee can modify activity name.")]
        public bool ActivityChangeName { get; set; } = true;

        [DisplayName("Enable project change. Asignee can move activity to another project.")]
        public bool ActivityChangeProject { get; set; } = true;

        [DisplayName("Enable status update. Asignee can change the activity status.")]
        public bool ActivityChangeStatus { get; set; } = true;

        [DisplayName("Enable priority change. Asignee can change the activity priority.")]
        public bool ActivityChangeProirity { get; set; } = true;

        [DisplayName("Enable team update. Asignee can change the activity team.")]
        public bool ActivityChangeTeam { get; set; } = true;

        [DisplayName("Enable estimated time change. Asignee can change the estimated time.")]
        public bool ActivityChangeEstimatedTime { get; set; } = true;

        [DisplayName("Enable activity type change. Asignee can modify activty type.")]
        public bool ActivityChangeActivityType { get; set; } = true;

        [DisplayName("Enable activity list change. Asignee can modify activity list.")]
        public bool ActivityChangeActivityList { get; set; } = true;

        [DisplayName("Enable sprint change. Asignee can modify activity sprint.")]
        public bool ActivityChangeSprint { get; set; } = true;

        [DisplayName("Enable start/due date change. Asignee can modify activity start date and due date.")]
        public bool ActivityChangeStartDueDate { get; set; } = true;

        [DisplayName("Enable delete. Asignee can delete activity.")]
        public bool ActivityDelete { get; set; } = true;

        #endregion

        #region Reports Notifications

        [DisplayName("Notify daily on logged time / day per team members.")]
        public bool DailyMembersLoggedTime { get; set; } = true;

        [DisplayName("Notify weekly on logged time / day per team members.")]
        public bool WeeklyMembersLoggedTime { get; set; } = true;

        [DisplayName("Notify monthly on logged time / day per team members.")]
        public bool MonthlyMembersLoggedTime { get; set; } = true;

        #endregion
    }
}