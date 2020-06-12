using System.ComponentModel.DataAnnotations;
using Gear.Identity.Permissions.Infrastructure.Attributes;

namespace Gear.Identity.Permissions.Domain.Resources
{
    public enum Permissions : short
    {
        NotSet = 0, //error condition

        #region PM-Projects

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "ProjectGroupCreate", Description = "Create a project group")]
        ProjectGroupCreate = 0x10,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "ProjectGroupUpdate", Description = "Update a project group")]
        ProjectGroupUpdate = 0x11,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "ProjectGroupDelete", Description = "Delete a project group")]
        ProjectGroupDelete = 0x12,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "ProjectGroupCreate", Description = "Create a project group")]
        ProjectGroupRead = 0x13,


        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "ProjectCreate", Description = "Create a project")]
        ProjectCreate = 0x14,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "ProjectUpdate", Description = "Update a project")]
        ProjectUpdate = 0x15,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "ProjectDelete", Description = "Delete a project")]
        ProjectDelete = 0x16,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "ProjectCreate", Description = "Create a project")]
        ProjectRead = 0x17,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "ProjectSetUp", Description = "Set up a project")]
        ProjectSetUp = 0x18,


        #endregion

        #region Sprints & Activity Lists

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityListCreate", Description = "Create a activity list")]
        ActivityListCreate = 0x19,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityListUpdate", Description = "Update a activity list")]
        ActivityListUpdate = 0x20,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityListDelete", Description = "Delete a activity list")]
        ActivityListDelete = 0x21,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityListCreate", Description = "Create a activity list")]
        ActivityListRead = 0x22,


        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "SprintCreate", Description = "Create a Sprint")]
        SprintCreate = 0x23,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "SprintUpdate", Description = "Update a Sprint")]
        SprintUpdate = 0x24,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "SprintDelete", Description = "Delete a Sprint")]
        SprintDelete = 0x25,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "SprintRead", Description = "Read a Sprint")]
        SprintRead = 0x26,

        #endregion

        #region Activity & changerequests

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "Activity Create", Description = "Create an Activity")]
        ActivityCreate = 0x27,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "Activity Update", Description = "Update an Activity")]
        ActivityUpdate = 0x28,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "Activity Delete", Description = "Delete an Activity")]
        ActivityDelete = 0x29,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "Activity Create", Description = "Create an Activity")]
        ActivityRead = 0x30,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityUpdateOtherUsersEntry", Description = "Update an Activity that belongs to another user.")]
        ActivityUpdateOtherUsersEntry = 0x31,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityDeleteOtherUsersEntry", Description = "Delete an Activity that belongs to another user.")]
        ActivityDeleteOtherUsersEntry = 0x32,
        

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ChangeRequest Create", Description = "Create a change request")]
        ChangeRequestCreate = 0x33,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ChangeRequest Update", Description = "Update a change request")]
        ChangeRequestUpdate = 0x34,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ChangeRequest Delete", Description = "Delete a change request")]
        ChangeRequestDelete = 0x35,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ChangeRequest Create", Description = "Create a change request")]
        ChangeRequestRead = 0x36,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ChangeRequest Approve", Description = "Approve incoming change request")]
        ChangeRequestApprove = 0x37,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ChangeRequest Reject", Description = "Reject incoming change request")]
        ChangeRequestReject = 0x38,

        #endregion

        #region Activity tracker & type

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityTypeCreate", Description = "Create a activity type")]
        ActivityTypeCreate = 0x39,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityTypeUpdate", Description = "Update a activity type")]
        ActivityTypeUpdate = 0x40,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityTypeDelete", Description = "Delete a activity type")]
        ActivityTypeDelete = 0x41,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityTypeCreate", Description = "Create a activity type")]
        ActivityTypeRead = 0x42,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Project Management", Name = "ActivityTrackerCreate", Description = "Create a activity tracker")]
        ActivityTrackerCreate = 0x43,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Project Management", Name = "ActivityTrackerUpdate", Description = "Update a activity tracker")]
        ActivityTrackerUpdate = 0x44,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Project Management", Name = "ActivityTrackerDelete", Description = "Delete a activity tracker")]
        ActivityTrackerDelete = 0x45,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Project Management", Name = "ActivityTrackerCreate", Description = "Create a activity tracker")]
        ActivityTrackerRead = 0x46,

        #endregion

        #region Log time & activity checklist

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "LogTimeCreate", Description = "Log time")]
        LogTimeCreate = 0x47,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "LogTimeUpdate", Description = "Update log time data")]
        LogTimeUpdate = 0x48,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "LogTimeDelete", Description = "Delete logged time data")]
        LogTimeDelete = 0x49,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ReadLogTime", Description = "See log time info")]
        LogTimeRead = 0x50,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "LogTimeDeleteOtherUsersEntry", Description = "Delete other users logged time")]
        LogTimeDeleteOtherUsersEntry = 0x51,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityCheckListItemCreate", Description = "Create an activity checklist item")]
        ActivityCheckListItemCreate = 0x52,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityCheckListItemUpdate", Description = "Update an activity checklist item")]
        ActivityCheckListItemUpdate = 0x53,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityCheckListItemDelete", Description = "Delete an activity checklist item")]
        ActivityCheckListItemDelete = 0x54,

        [LinkedToModule(PaidForModules.Activities)]
        [Display(GroupName = "Activity Management", Name = "ActivityCheckListItemCreate", Description = "Create an activity checklist item")]
        ActivityCheckListItemRead = 0x55,

        #endregion

        #region SSTP

        [LinkedToModule(PaidForModules.SSTP)]
        [Display(GroupName = "SSTP Management", Name = "SSTPCreate", Description = "Create an SSTP entity")]
        SSTPCreate = 0x56,

        [LinkedToModule(PaidForModules.SSTP)]
        [Display(GroupName = "SSTP Management", Name = "SSTPUpdate", Description = "Update an SSTP entity")]
        SSTPUpdate = 0x57,

        [LinkedToModule(PaidForModules.SSTP)]
        [Display(GroupName = "SSTP Management", Name = "SSTPDelete", Description = "Delete an SSTP entity")]
        SSTPDelete = 0x58,

        [LinkedToModule(PaidForModules.SSTP)]
        [Display(GroupName = "SSTP Management", Name = "SSTPCreate", Description = "Create an SSTP entity")]
        SSTPRead = 0x59,

        #endregion

        #region Wiki

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "WikiCreate", Description = "Create project wiki")]
        WikiCreate = 0x60,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "WikiRead", Description = "Read project wiki")]
        WikiRead = 0x61,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "WikiUpdate", Description = "Update project wiki")]
        WikiUpdate = 0x62,

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Project Management", Name = "WikiDelete", Description = "Delete project wiki")]
        WikiDelete = 0x63,

        #endregion

        #region Business Units and Departments

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "BusinessUnitCreate", Description = "Create a business unit")]
        BusinessUnitCreate = 0x64,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "BusinessUnitUpdate", Description = "Update a business unit")]
        BusinessUnitUpdate = 0x65,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "BusinessUnitDelete", Description = "Delete a business unit")]
        BusinessUnitDelete = 0x66,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "BusinessUnitCreate", Description = "Create a business unit")]
        BusinessUnitRead = 0x67,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "DepartmentCreate", Description = "Create a department")]
        DepartmentCreate = 0x68,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "DepartmentUpdate", Description = "Update a department")]
        DepartmentUpdate = 0x69,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "DepartmentDelete", Description = "Delete a department")]
        DepartmentDelete = 0x70,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "DepartmentCreate", Description = "Create a department")]
        DepartmentRead = 0x71,

        #endregion

        #region Department Teams and Employee

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "DepartmentTeamCreate", Description = "Create a department team")]
        DepartmentTeamCreate = 0x72,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "DepartmentTeamUpdate", Description = "Update a department team")]
        DepartmentTeamUpdate = 0x73,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "DepartmentTeamDelete", Description = "Delete a department team")]
        DepartmentTeamDelete = 0x74,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "DepartmentTeamCreate", Description = "Create a department team")]
        DepartmentTeamRead = 0x75,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "EmployeeCreate", Description = "Create a employee")]
        EmployeeCreate = 0x76,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "EmployeeUpdate", Description = "Update a employee")]
        EmployeeUpdate = 0x77,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "EmployeeDelete", Description = "Delete a employee")]
        EmployeeDelete = 0x78,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "EmployeeCreate", Description = "Create a employee")]
        EmployeeRead = 0x79,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "JobPositionCreate", Description = "Create a Job Position")]
        JobPositionCreate = 0x80,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "JobPositionUpdate", Description = "Update a Job Position")]
        JobPositionUpdate = 0x81,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "JobPositionDelete", Description = "Delete a Job Position")]
        JobPositionDelete = 0x82,

        [LinkedToModule(PaidForModules.ADM)]
        [Display(GroupName = "Employee Management", Name = "JobPositionCreate", Description = "Create a Job Position")]
        JobPositionRead = 0x83,


        #endregion

        #region Reports

        [LinkedToModule(PaidForModules.PM)]
        [Display(GroupName = "Reports Management", Name = "ReportsManagement", Description = "View, generate and download reports")]
        ReportsManagement = 0x84,

        #endregion

        #region Notifications

        [LinkedToModule(PaidForModules.Notifications)]
        [Display(GroupName = "Notifications Management", Name = "NotificationProfileCreate", Description = "Create a notification profile")]
        NotificationProfileCreate = 0xe,

        [LinkedToModule(PaidForModules.Notifications)]
        [Display(GroupName = "Notifications Management", Name = "NotificationProfileUpdate", Description = "Update a notification profile")]
        NotificationProfileUpdate = 0xf,

        [LinkedToModule(PaidForModules.Notifications)]
        [Display(GroupName = "Notifications Management", Name = "NotificationProfileDelete", Description = "Delete a notification profile")]
        NotificationProfileDelete = 0x1a,

        [LinkedToModule(PaidForModules.Notifications)]
        [Display(GroupName = "Notifications Management", Name = "NotificationProfileCreate", Description = "Create a notification profile")]
        NotificationProfileRead = 0x90,

        #endregion

        #region Permissions

        [LinkedToModule(PaidForModules.Permissions)]
        [Display(GroupName = "Permissions Management", Name = "RoleCreate", Description = "Create a role")]
        RoleCreate = 0x1c,

        [LinkedToModule(PaidForModules.Permissions)]
        [Display(GroupName = "Permissions Management", Name = "RoleUpdate", Description = "Update a role")]
        RoleUpdate = 0x1d,

        [LinkedToModule(PaidForModules.Permissions)]
        [Display(GroupName = "Permissions Management", Name = "RoleDelete", Description = "Delete a role")]
        RoleDelete = 0x1e,

        [LinkedToModule(PaidForModules.Permissions)]
        [Display(GroupName = "Permissions Management", Name = "RoleCreate", Description = "Create a role")]
        RoleRead = 0x1f,

        [LinkedToModule(PaidForModules.Permissions)]
        [Display(GroupName = "Permissions Management", Name = "RoleCreate", Description = "Assign a role")]
        RoleAssign = 0x2a,

        #endregion
    }
}
