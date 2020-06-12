using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bizon360.Models;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Domain.ReportEntities.Enums;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportActivityStatuses;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportDate;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportGuids;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportActivityStatuses;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportDate;
using Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportGuidList;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.ChangeUserreportState;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsByName;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportDetails;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.ActivateTableHeader;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.ActivateTableHeaderList;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetReportTableHeaders;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetUserReportHeaders;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers.ExcelTableHelpers;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Identity.Permissions.Infrastructure.Utils;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectActivityTypeTrackers;
using Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectOverallProgress;
using Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByEmployeesReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByPriorityReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByStatusesReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByTypeReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetActivityListByProjectReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralLoggedTimeByPeriodList;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralSprintListByProjectReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetLoggedTimeByPeriodList;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGeneralReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGroupsGeneralReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectLoggedTimeReportList;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetSprintListReportFromSelected;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTime;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeList;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupList;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectList;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsByGroup;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsTeams;
using Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintListQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    /// <summary>
    /// Returns the report file on the specified link (example, (domain)//GetEmployeeLoggedTimeCsv?employeeId=(Guid)&[another_params].
    /// If th that the input data in URL contained invalid info or other possible error: (StringBuilder exceptions, for example).
    /// In order to understand how does files are returned see: Controller.Response Property and HttpResponseBase class documentation:
    /// https://docs.microsoft.com/en-us/dotnet/api/system.web.httpresponsebase?view=netframework-4.8
    /// </summary>
    [Authorize]
    public class ReportsController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Models.Platform.Pm;
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public async Task<IActionResult> ReportsIndex()
        {
            await GetProjects();

            var headers = new List<string> { "#", "Project", "Start Date", "Deadline", "Est.", "Log." };
            ViewBag.Headers = headers.Select(x => new SelectListItem
            {
                Text = x,
                Value = x,
                Selected = true
            });

            return View();
        }

        private async Task GetProjects()
        {
            var getAllProjects = User.UserHasThisPermission(Permissions.ProjectUpdate);

            var projects = await Mediator.Send(new GetProjectListQuery { GetAllProjects = getAllProjects });

            ViewBag.Projects = projects.Projects.OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                .ToList();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public IActionResult GeneralReport()
        {
            var projectGroups = Mediator.Send(new GetProjectGroupListQuery { HasAccessToAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate) }).Result;
            if (projectGroups != null)
            {
                ViewBag.ProjectGroups = projectGroups.ProjectGroups.Select(pg => new SelectListItem
                {
                    Value = pg.Id.ToString(),
                    Text = pg.Name
                }).ToList();
            }

            var tableHelper = new ExcelTableHelper();

            ViewBag.GeneralHeaders = tableHelper.GetGeneralFilteredReportTableHeaders().Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name,
                Disabled = !x.IsRemovable,
                Selected = x.IsActive || !x.IsRemovable
            }).Append(new SelectListItem
            {
                Value = "Created",
                Text = "Created",
                Selected = true
            }).ToList();

            return View();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public async Task<IActionResult> EmployeesReport()
        {
            await GetProjects();

            var headers = new List<string> { "#", "Employee", "Activity", "Project", "Activity status", "Est.", "Log.", "Created" };
            ViewBag.Headers = headers.Select(x => new SelectListItem
            {
                Text = x,
                Value = x,
                Selected = true
            });


            return View();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public async Task<IActionResult> ActivitiesReport()
        {
            await GetProjects();

            var activityTypes = await Mediator.Send(new GetActivityTypeListQuery());

            var tableHelper = new ExcelTableHelper();

            ViewBag.FilteredReportTableHeaders = tableHelper.GetProjectsByFiltersReportTableHeaders().Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name,
                Disabled = !x.IsRemovable,
                Selected = x.IsActive || !x.IsRemovable
            }).Append(new SelectListItem
            {
                Value = "Created",
                Text = "Created",
                Selected = true
            }).ToList();

            ViewBag.ActivityTypes = activityTypes.ActivityTypes.OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            return View();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public async Task<IActionResult> HistoryReport()
        {
            var projectGroups = Mediator.Send(new GetProjectGroupListQuery { HasAccessToAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate) }).Result;
            if (projectGroups != null)
            {
                ViewBag.ProjectGroups = projectGroups.ProjectGroups.Select(pg => new SelectListItem
                {
                    Value = pg.Id.ToString(),
                    Text = pg.Name
                }).ToList();
            }

            var activityTypes = await Mediator.Send(new GetActivityTypeListQuery());

            var tableHelper = new ExcelTableHelper();

            ViewBag.FilteredHistoryReportTableHeaders = tableHelper.GetProjectGroupsWithHistoryFilteredReportTableHeaders().Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name,
                Disabled = !x.IsRemovable,
                Selected = x.IsActive || !x.IsRemovable
            }).ToList();

            ViewBag.ActivityTypes = activityTypes.ActivityTypes.OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            return View();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public async Task<IActionResult> TeamsReport()
        {
            await GetProjects();

            var activityTypes = await Mediator.Send(new GetActivityTypeListQuery());

            ViewBag.ActivityTypes = activityTypes.ActivityTypes.OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            var tableHelper = new ExcelTableHelper();

            ViewBag.FilteredTeamsReportTableHeaders = tableHelper.GetTeamsByFiltersReportTableHeaders().Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name,
                Disabled = !x.IsRemovable,
                Selected = x.IsActive || !x.IsRemovable
            }).Append(new SelectListItem
                {
                    Value = "Created",
                    Text = "Created",
                    Selected = true
                }).ToList();

            return View();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public async Task<IActionResult> SprintsReport()
        {
            await GetProjects();

            ViewBag.Sprints = new List<SelectListItem>();

            var tableHelper = new ExcelTableHelper();
            ViewBag.TableHeaders = tableHelper.GetSprintListGeneralReportTableHeaders().Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name,
                Disabled = !x.IsRemovable,
                Selected = x.IsActive || !x.IsRemovable
            });

            return View();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public IActionResult Settings()
        {
            return View();
        }


        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public async Task<IActionResult> OverdueSettings(string reportName, IList<Guid> projectIds, IList<Guid> userIds, 
            IList<Guid> overdueHeaders, DateTime startDate, string enabledOverdue)
        {
            Guid.TryParse(_userManager.GetUserId(User), out var userId);

            var report = await Mediator.Send(new GetReportDetailsByNameQuery { Name = reportName });

            await Mediator.Send(new UpdateUserReportGuidsCommand<Guid>
            {
                ReportId = report.Id, UserId = userId, GuidList = projectIds, FilterType = FilterType.ProjectIds
            });

            await Mediator.Send(new UpdateUserReportGuidsCommand<Guid>
            {
                ReportId = report.Id,
                UserId = userId,
                GuidList = userIds,
                FilterType = FilterType.EmployeeIds
            });

            await Mediator.Send(new UpdateUserReportDateCommand<Guid>
                {Date = startDate, FilterType = FilterType.StartDate, ReportId = report.Id, UserId = userId});

            await Mediator.Send(new ActivateTableHeaderListCommand
                {ReportId = report.Id, UserId = userId, TableHeaderList = overdueHeaders});

            await Mediator.Send(new ChangeUserReportStateCommand
                {ReportId = report.Id, UserId = userId, Active = (enabledOverdue == "true")});

            return RedirectToAction("Settings");
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> TeamSettings(string reportName, IList<Guid> projectIds, IList<Guid> userIds, IList<Guid> teamHeaders, 
            IList<Guid> activityTypes, IList<ActivityStatus> activityStatuses, DateTime startDate, string enabledTeams)
        {
            Guid.TryParse(_userManager.GetUserId(User), out var userId);

            var report = await Mediator.Send(new GetReportDetailsByNameQuery { Name = reportName });

            await Mediator.Send(new UpdateUserReportGuidsCommand<Guid>
            {
                ReportId = report.Id,
                UserId = userId,
                GuidList = projectIds,
                FilterType = FilterType.ProjectIds
            });

            await Mediator.Send(new UpdateUserReportGuidsCommand<Guid>
            {
                ReportId = report.Id,
                UserId = userId,
                GuidList = userIds,
                FilterType = FilterType.EmployeeIds
            });

            await Mediator.Send(new UpdateUserReportGuidsCommand<Guid>
            {
                ReportId = report.Id,
                UserId = userId,
                GuidList = activityTypes,
                FilterType = FilterType.ActivityTypeIds
            });

            await Mediator.Send(new UpdateUserReportDateCommand<Guid>
            { Date = startDate, FilterType = FilterType.StartDate, ReportId = report.Id, UserId = userId });

            await Mediator.Send(new ActivateTableHeaderListCommand
            { ReportId = report.Id, UserId = userId, TableHeaderList = teamHeaders });

            await Mediator.Send(new UpdateUserReportActivityStatusesCommand<Guid>
            {
                ReportId = report.Id,
                UserId = userId,
                ActivityStatuses = activityStatuses
            });

            await Mediator.Send(new ChangeUserReportStateCommand
                {ReportId = report.Id, UserId = userId, Active = (enabledTeams == "true") });

            return RedirectToAction("Settings");
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public async Task<IActionResult> ProjectSettings(string reportName, IList<Guid> projectIds, IList<Guid> projectHeaders,
            IList<Guid> activityTypeIds, IList<ActivityStatus> activityStatuses, DateTime startDate, string enabledProjects)
        {
            Guid.TryParse(_userManager.GetUserId(User), out var userId);

            var report = await Mediator.Send(new GetReportDetailsByNameQuery { Name = reportName });

            await Mediator.Send(new UpdateUserReportGuidsCommand<Guid>
            {
                ReportId = report.Id,
                UserId = userId,
                GuidList = projectIds,
                FilterType = FilterType.ProjectIds
            });

            await Mediator.Send(new UpdateUserReportGuidsCommand<Guid>
            {
                ReportId = report.Id,
                UserId = userId,
                GuidList = activityTypeIds,
                FilterType = FilterType.ActivityTypeIds
            });

            await Mediator.Send(new UpdateUserReportDateCommand<Guid>
                { Date = startDate, FilterType = FilterType.StartDate, ReportId = report.Id, UserId = userId });

            await Mediator.Send(new ActivateTableHeaderListCommand
                { ReportId = report.Id, UserId = userId, TableHeaderList = projectHeaders });

            await Mediator.Send(new UpdateUserReportActivityStatusesCommand<Guid>
            {
                ReportId = report.Id,
                UserId = userId,
                ActivityStatuses = activityStatuses
            });

            await Mediator.Send(new ChangeUserReportStateCommand
                { ReportId = report.Id, UserId = userId, Active = (enabledProjects == "true") });

            return RedirectToAction("Settings");
        }

        #region Customization Filters
        public async Task<List<SelectListItem>> GetProjectFilters(Guid userId, Guid reportId)
        {
            var getAllProjects = User.UserHasThisPermission(Permissions.ProjectUpdate);

            var projects = await Mediator.Send(new GetProjectListQuery { GetAllProjects = getAllProjects });

            var selectedProjects = await Mediator.Send(new GetUserReportGuidListQuery { UserId = userId, ReportId = reportId, FilterType = FilterType.ProjectIds });

            return projects.Projects.OrderBy(x => x.Name)
               .Select(x => new SelectListItem { 
                   Value = x.Id.ToString(), 
                   Text = x.Name,
                   Selected = selectedProjects != null && selectedProjects.GuidList.Contains(x.Id)
               })
               .ToList();
        }

        public async Task<List<SelectListItem>> GetEmployeeFilters(Guid userId, Guid reportId)
        {
            var employees = await Mediator.Send(new GetApplicationUserListQuery());

            var selectedEmployees = await Mediator.Send(new GetUserReportGuidListQuery { UserId = userId, ReportId = reportId, FilterType = FilterType.EmployeeIds });

            return employees.ApplicationUsers
                .OrderBy(x => $"{x.FirstName} {x.LastName}")
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.FirstName} {x.LastName}",
                    Selected = selectedEmployees != null && selectedEmployees.GuidList.Contains(x.Id)
                })
                .Distinct()
                .ToList();
        }

        public async Task<List<SelectListItem>> GetActivityTypeFilters(Guid userId, Guid reportId)
        {
            var activityTypes = await Mediator.Send(new GetActivityTypeListQuery());

            var selectedActivityTypes = await Mediator.Send(new GetUserReportGuidListQuery { UserId = userId, ReportId = reportId, FilterType = FilterType.ActivityTypeIds });

            return activityTypes.ActivityTypes.OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = selectedActivityTypes != null && selectedActivityTypes.GuidList.Contains(x.Id)
                }).ToList();
        }

        public async Task<List<SelectListItem>> GetActivityStatusFilters(Guid userId, Guid reportId)
        {
            var activityStatuses = new List<ActivityStatus> { ActivityStatus.Completed, ActivityStatus.Developed,
                        ActivityStatus.InProgress, ActivityStatus.New, ActivityStatus.Refused, ActivityStatus.Tested };

            var selectedActivityStatuses = await Mediator.Send(new GetUserReportActivityStatusesQuery { UserId = userId, ReportId = reportId });

            var result = activityStatuses
                .Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString(),
                    Selected = selectedActivityStatuses != null && selectedActivityStatuses.ActivityStatuses.Contains(x)
                }).ToList();

            return result;
        }

        public async Task<UserReportDate> GetStartDateFilter(Guid userId, Guid reportId)
        {
            return await Mediator.Send(new GetUserReportDateQuery { UserId = userId, ReportId = reportId, FilterType = FilterType.StartDate });
        }

        public async Task<UserReportDate> GetDueDateFilter(Guid userId, Guid reportId)
        {
            return await Mediator.Send(new GetUserReportDateQuery { UserId = userId, ReportId = reportId, FilterType = FilterType.DueDate });
        }

        public async Task<List<SelectListItem>> GetHeaders(Guid userId, Guid reportId, IEnumerable<string> allowedHeaders)
        {
            var userReportHeaders = await Mediator.Send(new GetUserReportHeadersQuery { ReportId = reportId, UserId = userId });

            return userReportHeaders.TableHeaders
                .OrderBy(x => x.Id).Distinct()
                .Where(x => allowedHeaders.Contains(x.Name))
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Active
                }).ToList();
        }

        public async Task<IActionResult> IsActiveReport(string reportName)
        {
            Guid.TryParse(_userManager.GetUserId(User), out var userId);

            var report = await Mediator.Send(new GetReportDetailsByNameQuery { Name = reportName });

            var userReport = await Mediator.Send(new GetUserReportDetailsQuery {ReportId = report.Id, UserId = userId});

            return new JsonResult(userReport.Active);
        }

        #endregion

        #region Settings
        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetDailyLogsByTeams()
        {
            Guid.TryParse(_userManager.GetUserId(User), out var userId);

            var report = await Mediator.Send(new GetReportDetailsByNameQuery { Name = "Teams by filters" });

            ViewBag.SelectedProjects = await GetProjectFilters(userId, report.Id);

            ViewBag.SelectedEmployees = await GetEmployeeFilters(userId, report.Id);

            ViewBag.SelectedActivityTypes = await GetActivityTypeFilters(userId, report.Id);

            ViewBag.SelectedActivityStatuses = await GetActivityStatusFilters(userId, report.Id);

            var excelHelper = new ExcelTableHelper();
            var allowedHeaders = excelHelper.GetTeamsByFiltersReportTableHeaders().Select(x => x.Name);

            ViewBag.TeamHeaders = await GetHeaders(userId, report.Id, allowedHeaders);

            return PartialView("SettingsPartials/_dailyLogsTeams");
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetDailyLogsByProjects()
        {
            Guid.TryParse(_userManager.GetUserId(User), out var userId);

            var report = await Mediator.Send(new GetReportDetailsByNameQuery { Name = "Projects by filters" });

            ViewBag.SelectedProjectsViaProjects = await GetProjectFilters(userId, report.Id);

            ViewBag.SelectedActivityTypesViaProjects = await GetActivityTypeFilters(userId, report.Id);

            ViewBag.SelectedActivityStatusesViaProjects = await GetActivityStatusFilters(userId, report.Id);

            var excelHelper = new ExcelTableHelper();
            var allowedHeaders = excelHelper.GetProjectsByFiltersReportTableHeaders().Select(x => x.Name);

            ViewBag.ProjectHeaders = await GetHeaders(userId, report.Id, allowedHeaders);

            return PartialView("SettingsPartials/_dailyLogsProjects");
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetOverdue()
        {
            Guid.TryParse(_userManager.GetUserId(User), out var userId);

            var report = await Mediator.Send(new GetReportDetailsByNameQuery { Name = "Overdue" });

            ViewBag.SelectedProjectsOverdue = await GetProjectFilters(userId, report.Id);

            ViewBag.SelectedEmployeesOverdue = await GetEmployeeFilters(userId, report.Id);

            ViewBag.StartDateOverdue = await GetStartDateFilter(userId, report.Id);

            ViewBag.DueDateOverdue = await GetDueDateFilter(userId, report.Id);

            var excelHelper = new ExcelTableHelper();
            var allowedHeaders = excelHelper.GetOverdueTasksFilteredReportTableHeaders().Select(x => x.Name);

            ViewBag.OverdueHeaders = await GetHeaders(userId, report.Id, allowedHeaders);

            return PartialView("SettingsPartials/_overdue");
        }

        #endregion

        #region General report
        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetGeneralReportXml(List<Guid> groupsIds, List<Guid> projectsIds, 
            List<Guid> teamsIds,  DateTime startDate, DateTime dueDate, List<string> tableHeaders)
        {
            var request = await Mediator.Send(new GetGeneralByFiltersReportQuery
            {
                GroupsIds = groupsIds,
                ProjectsIds = projectsIds,
                TeamsIds = teamsIds,
                StartDate = startDate,
                EndDate = dueDate
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetGeneralFilteredReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetGeneralFilteredReport(request, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetGeneralReportCsv(List<Guid> groupsIds, List<Guid> projectsIds, 
            List<Guid> teamsIds, DateTime startDate, DateTime dueDate, List<string> tableHeaders)
        {
            var request = await Mediator.Send(new GetGeneralByFiltersReportQuery
            {
                GroupsIds = groupsIds,
                ProjectsIds = projectsIds,
                TeamsIds = teamsIds,
                StartDate = startDate,
                EndDate = dueDate
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetGeneralFilteredReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetGeneralFilteredReport(request, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }
        #endregion

        #region General project reports

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<ActionResult> GetProjectsReportUi(List<Guid> projectIds, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectLoggedTimeReportListQuery
            {
                ProjectIds = projectIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            return PartialView("_ProjectsReport", request);
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectListReportXml(List<Guid> projectIds, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectLoggedTimeReportListQuery
            {
                ProjectIds = projectIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var excelReportHelper = new ExcelReportHelper();

            var result = excelReportHelper.GetProjectListReport(request, startDate, dueDate);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }
        
        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectListReportCsv(List<Guid> projectIds, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectLoggedTimeReportListQuery
            {
                ProjectIds = projectIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetProjectListReport(request, startDate, dueDate);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }
        
        #endregion

        #region General employees report

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetEmployeesLoggedTimeOnUi(List<Guid> userIds, List<Guid> projectIds, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetEmployeeFiltersReportQuery
            {
                UserIds = userIds,
                ProjectIds = projectIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            return PartialView("_EmployeesReport", request);
        }


        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetEmployeesLoggedTimeByPeriodXml(List<Guid> userIds, List<Guid> projectIds, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetEmployeeFiltersReportQuery
            {
                UserIds = userIds,
                ProjectIds = projectIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetEmployeeFilteredReport(request, tableHelper.GetTeamsByFiltersReportTableHeaders());

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetEmployeesLoggedTimeByPeriodCsv(List<Guid> userIds, List<Guid> projectIds, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetEmployeeFiltersReportQuery
            {
                UserIds = userIds,
                ProjectIds = projectIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetEmployeeFilteredReport(request, tableHelper.GetTeamsByFiltersReportTableHeaders());

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }
        
        #endregion

        #region Project report

        [HttpPost]
        public async Task<IActionResult> GenerateProjectReport(List<Guid> activityListIds, Guid projectId)
        {
            var request = await Mediator.Send(new GetActivityListByProjectReportQuery
            {
                ProjectId = projectId,
                ActivityListIds = activityListIds
            });

            if (request == null) return BadRequest();

            return PartialView("_GenerateProjectReport", request);
        }

        [HttpPost]
        public async Task<IActionResult> GetActivityListsByProjectReportXml(List<Guid> activityListIds, Guid projectId)
        {
            var request = await Mediator.Send(new GetActivityListByProjectReportQuery
            {
                ProjectId = projectId,
                ActivityListIds = activityListIds
            });

            if (request == null) return BadRequest();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetActivityListsByProjectReport(request);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetActivityListsByProjectReportCsv(List<Guid> activityListIds, Guid projectId)
        {
            var request = await Mediator.Send(new GetActivityListByProjectReportQuery
            {
                ProjectId = projectId,
                ActivityListIds = activityListIds
            });

            if (request == null) return BadRequest();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetActivityListsByProjectReport(request);

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetActivityListsByProjectReportPdf(Guid projectId)
        {
            var request = await Mediator.Send(new GetActivityListByProjectReportQuery
            {
                ProjectId = projectId
            });

            if (request == null) return BadRequest();

            var pdfReportHelper = new PdfReportHelper();
            var result = pdfReportHelper.GetActivityListsByProjectReport(request);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "PdfProjectReport.pdf");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        #endregion

        #region Sprints report
    
        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetSprintListGeneralBySelectedReportOnUi(Guid projectId, List<Guid> sprintIds)
        {
            var request = await Mediator.Send(new GetSprintListReportFromSelectedQuery
            {
                Id = projectId,
                SprintIds = sprintIds
            });

            if (request == null) return BadRequest();

            return PartialView("_SprintsReport", request);
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetSprintListGeneralBySelectedReportXml(Guid projectId, List<Guid> sprintIds, List<string> tableHeaders)
        {
            var request = await Mediator.Send(new GetSprintListReportFromSelectedQuery
            {
                Id = projectId,
                SprintIds = sprintIds
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetSprintListGeneralReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetSprintListGeneralReport(request, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetSprintListGeneralBySelectedReportCsv(Guid projectId, List<Guid> sprintIds, IList<string> tableHeaders)
        {
            var request = await Mediator.Send(new GetSprintListReportFromSelectedQuery
            {
                Id = projectId,
                SprintIds = sprintIds
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetSprintListGeneralReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetSprintListGeneralReport(request, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetSprints(Guid projectId)
        {
            var request = await Mediator.Send(new GetSprintListQuery {ProjectId = projectId});
            
            if (request != null)
            {
                return Json(request.Sprints.Select(u => new
                {
                    value = u.Id.ToString(),
                    text = u.Name
                }).ToList());
            }

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectsByGroup(List<Guid> projectGroupIds)
        {
            bool isEmpty =  projectGroupIds == null || !projectGroupIds.Any();

            if (isEmpty)
                return NotFound();

            var request = await Mediator.Send(new GetProjectsByGroupsQuery { GroupIds = projectGroupIds });

            if (request != null)
            {
                return Json(request.Projects.Select(p => new
                {
                    value = p.Id.ToString(),
                    text = p.Name
                }).ToList());
            }

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetpTeamsByProjects(List<Guid> projectsIds)
        {
            bool isEmpty = projectsIds == null || !projectsIds.Any();

            if (isEmpty)
                return NotFound();

            var request = await Mediator.Send(new GetProjectsTeamsQuery { ProjectsIds = projectsIds });

            if (request != null)
            {
                return Json(request.Users.Select(u => new
                {
                    value = u.Id.ToString(),
                    text = $"{u.FirstName} {u.LastName}"
                }).ToList());
            }

            return BadRequest();
        }
        #endregion

        #region History report
        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GenerateHistoryReport(List<Guid> assigneeIds, List<Guid> activityTypeIds,
            List<ProjectStatus> projectStatuses, Interval intervalType, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectGroupsByFiltersQuery
            {
                AssigneeIds = assigneeIds,
                ActivityTypeIds = activityTypeIds,
                StartDate = startDate,
                DueDate = dueDate,
                ProjectStatuses = projectStatuses,
                IntervalType = intervalType
            });

            if (request == null) return BadRequest();

            return PartialView("_GenerateHistoryReport", request);
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectGroupsHistoryReportXlm(List<Guid> assigneeIds,
            List<Guid> activityTypeIds, List<ProjectStatus> projectStatuses, Interval intervalType, DateTime startDate,
            DateTime dueDate, List<string> tableHeaders)
        {
            var request = await Mediator.Send(new GetProjectGroupsByFiltersQuery
            {
                AssigneeIds = assigneeIds,
                ActivityTypeIds = activityTypeIds,
                StartDate = startDate,
                DueDate = dueDate,
                ProjectStatuses = projectStatuses,
                IntervalType = intervalType
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetProjectGroupsWithHistoryFilteredReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetProjectGroupsFilteredReportWithHistory(request, startDate, dueDate, intervalType, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectGroupsHistoryReportCsv(List<Guid> assigneeIds,
            List<Guid> activityTypeIds, List<ProjectStatus> projectStatuses, Interval intervalType, DateTime startDate,
            DateTime dueDate, List<string> tableHeaders)
        {
            var request = await Mediator.Send(new GetProjectGroupsByFiltersQuery
            {
                AssigneeIds = assigneeIds,
                ActivityTypeIds = activityTypeIds,
                StartDate = startDate,
                DueDate = dueDate,
                ProjectStatuses = projectStatuses,
                IntervalType = intervalType
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetProjectGroupsWithHistoryFilteredReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetProjectGroupsFilteredReportWithHistory(request, startDate, dueDate, intervalType, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }


        #endregion

        #region project filtered (activity) report
        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GenerateFilteredProjectReport(List<Guid> projectIds, List<Guid> activityTypeIds, 
            List<ActivityStatus> activityStatuses, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectsByFiltersReportQuery
            {
                ProjectIds = projectIds,
                ActivityTypes = activityTypeIds,
                ActivityStatuses = activityStatuses,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            return PartialView("_GenerateFilteredProjectReport", request);
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetFilteredProjectReportXml(List<Guid> projectIds, List<Guid> activityTypeIds,
            List<ActivityStatus> activityStatuses, DateTime startDate, DateTime dueDate, IList<string> tableHeaders)
        {
            var request = await Mediator.Send(new GetProjectsByFiltersReportQuery
            {
                ProjectIds = projectIds,
                ActivityTypes = activityTypeIds,
                ActivityStatuses = activityStatuses,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetProjectsByFiltersReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetProjectsByFiltersReport(request, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetFilteredProjectReportCsv(List<Guid> projectIds, List<Guid> activityTypeIds,
            List<ActivityStatus> activityStatuses, DateTime startDate, DateTime dueDate, IList<string> tableHeaders)
        {
            var request = await Mediator.Send(new GetProjectsByFiltersReportQuery
            {
                ProjectIds = projectIds,
                ActivityTypes = activityTypeIds,
                ActivityStatuses = activityStatuses,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetProjectsByFiltersReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var excelReportHelper = new CsvReportHelper();
            var result = excelReportHelper.GetProjectsByFiltersReport(request, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetFilteredProjectReportOnUi(List<Guid> projectIds, List<Guid> activityTypeIds,
            List<ActivityStatus> activityStatuses, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectsByFiltersReportQuery
            {
                ProjectIds = projectIds,
                ActivityTypes = activityTypeIds,
                ActivityStatuses = activityStatuses,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            return PartialView("_ActivitiesReport", request);
        }
        #endregion

        #region Teams filtered report
        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GenerateFilteredTeamsReport(IList<Guid> employeeIds, IList<Guid> activityTypeIds,
            IList<ActivityStatus> activityStatuses, IList<Guid> projectIds, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetTeamsByFiltersReportQuery
            {
                ProjectIds = projectIds,
                ActivityStatuses = activityStatuses,
                ActivityTypeIds = activityTypeIds,
                EmployeeIds = employeeIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            return PartialView("_GenerateFilteredTeamsReport", request);
        }

        public async Task<IActionResult> GenerateFilteredTeamsReportXml(IList<Guid> employeeIds, IList<Guid> activityTypeIds,
            IList<ActivityStatus> activityStatuses, IList<Guid> projectIds, IList<string> tableHeaders, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetTeamsByFiltersReportQuery
            {
                ProjectIds = projectIds,
                ActivityStatuses = activityStatuses,
                ActivityTypeIds = activityTypeIds,
                EmployeeIds = employeeIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetTeamsByFiltersReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetTeamsByFiltersReport(request, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }

        public async Task<IActionResult> GenerateFilteredTeamsReportCsv(IList<Guid> employeeIds,
            IList<Guid> activityTypeIds,
            IList<ActivityStatus> activityStatuses, IList<Guid> projectIds, IList<string> tableHeaders,
            DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetTeamsByFiltersReportQuery
            {
                ProjectIds = projectIds,
                ActivityStatuses = activityStatuses,
                ActivityTypeIds = activityTypeIds,
                EmployeeIds = employeeIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetTeamsByFiltersReportTableHeaders()
                .Where(x => tableHeaders.Contains(x.Name)).ToList();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetTeamsByFiltersReport(request, headers);

            if (result == null) return BadRequest();

            TempData["Download_reportByte"] = result;

            return Ok();
        }
        #endregion

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetEmployeeLoggedTimeXml(Guid employeeId, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetUserLoggedTimeQuery
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetEmployeeLoggedTime(request, startDate, dueDate);

            if (result == null)
            {
                return BadRequest();
            }

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetEmployeeLoggedTimeCsv(Guid employeeId, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetUserLoggedTimeQuery
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetEmployeeLoggedTime(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "CsvReport.CSV");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetEmployeeLoggedTimePdf(Guid employeeId, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetUserLoggedTimeQuery
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var pdfReportHelper = new PdfReportHelper();
            var result = pdfReportHelper.GetEmployeeLoggedTime(request, startDate, dueDate);

            if (result == null)
            {
                return BadRequest();
            }

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "PdfProjectReport.pdf");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetGeneralLoggedTimeXml(DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetGeneralLoggedTimeByPeriodListQuery
            {
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetEmployeesLoggedTimeByPeriod(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelPeriodReport.xlsx");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetGeneralLoggedTimeCsv(DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetGeneralLoggedTimeByPeriodListQuery
            {
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetEmployeesLoggedTimeByPeriod(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "CsvReport.CSV");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetGeneralLoggedTimePdf(DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetGeneralLoggedTimeByPeriodListQuery
            {
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var pdfReportHelper = new PdfReportHelper();
            var result = pdfReportHelper.GetEmployeesLoggedTimeByPeriod(request, startDate, dueDate);

            if (result == null)
            {
                return BadRequest();
            }

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "PdfProjectReport.pdf");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetEmployeesLoggedTimeByPeriodPdf(List<Guid> userIds, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetLoggedTimeByPeriodListQuery
            {
                UserIds = userIds,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var pdfReportHelper = new PdfReportHelper();
            var result = pdfReportHelper.GetEmployeesLoggedTimeByPeriod(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "PdfProjectReport.pdf");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectGeneralReportXml(Guid projectId, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectGeneralReportQuery
            {
                ProjectId = projectId,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetProjectGeneralReport(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelProjectReport.xlsx");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectGeneralReportCsv(Guid projectId, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectGeneralReportQuery
            {
                ProjectId = projectId,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetProjectGeneralReport(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "CsvProjectReport.CSV");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectGeneralReportPdf(Guid projectId, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectGeneralReportQuery
            {
                ProjectId = projectId,
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var pdfReportHelper = new PdfReportHelper();
            var result = pdfReportHelper.GetProjectGeneralReport(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "PdfProjectReport.pdf");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectGroupsReportXml(DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectGroupsGeneralReportQuery
            {
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetProjectGroupsReport(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelProjectGroupsReport.xlsx");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectGroupsReportCsv(DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectGroupsGeneralReportQuery
            {
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetProjectGroupsListReport(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "CsvProjectGroupsReport.CSV");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetProjectGroupsReportPdf(DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetProjectGroupsGeneralReportQuery
            {
                StartDate = startDate,
                DueDate = dueDate
            });

            if (request == null) return BadRequest();

            var pdfReportHelper = new PdfReportHelper();
            var result = pdfReportHelper.GetProjectGroupsListReport(request, startDate, dueDate);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "PdfProjectReport.pdf");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetSprintListGeneralReportXml(Guid projectId)
        {
            var request = await Mediator.Send(new GetGeneralSprintListByProjectReportQuery
            {
                Id = projectId
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var tableHeaders = tableHelper.GetSprintListGeneralReportTableHeaders();
            tableHeaders[2].IsActive = false;
            tableHeaders[4].Order = 3;
            tableHeaders[3].Order = 4;

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetSprintListGeneralReport(request, tableHeaders);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelProjectSprintsReport.xlsx");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> GetSprintListGeneralReportCsv(Guid projectId)
        {
            var request = await Mediator.Send(new GetGeneralSprintListByProjectReportQuery
            {
                Id = projectId
            });

            if (request == null) return BadRequest();

            var csvReportHelper = new CsvReportHelper();
            var result = csvReportHelper.GetSprintListGeneralReport(request, null);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "CsvSprintsReport.csv");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectsByFiltersReportXml()
        {
            var request = await Mediator.Send(new GetProjectsByFiltersReportQuery
            {
                ProjectIds = new List<Guid> { Guid.Parse("6CE55503A60642A4B79311E3BCD2B438"), Guid.Parse("EAA39DDE67AF4C7182FC53A07D614C72"), Guid.Parse("762DF025A0694427B764703B26D11D2C") },
                ActivityStatuses = new List<ActivityStatus> { ActivityStatus.Completed, ActivityStatus.Developed, ActivityStatus.InProgress, ActivityStatus.InProgress, ActivityStatus.New, ActivityStatus.Refused, ActivityStatus.Tested },
                ActivityTypes = new List<Guid> { Guid.Parse("886912C21B4C4D5C9E1A17613CEAF0F5"), Guid.Parse("982CBE71360C4715A218C7ABD6073B97"), Guid.Parse("C5C5167AC50B4526B9C1CD207B23CC75"), Guid.Parse("C14519AD26D7402998DDD0498C90318B") },
                StartDate = new DateTime(2018, 1, 1),
                DueDate = DateTime.Now,
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var tableHeaders = tableHelper.GetProjectsByFiltersReportTableHeaders();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetProjectsByFiltersReport(request, tableHeaders);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelProjectSprintsReport.xlsx");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTeamsByFiltersReport()
        {
            var request = await Mediator.Send(new GetTeamsByFiltersReportQuery
            {
                ProjectIds = new List<Guid> { Guid.Parse("762DF025A0694427B764703B26D11D2C"), Guid.Parse("EAA39DDE67AF4C7182FC53A07D614C72") },
                ActivityStatuses = new List<ActivityStatus> { ActivityStatus.InProgress, ActivityStatus.New, ActivityStatus.Completed },
                EmployeeIds = new List<Guid> { Guid.Parse("338E8954B9E24281AE33ADA59CCD4C25"), Guid.Parse("9525D234-DB98-4411-A69B-D130810D8138"), Guid.Parse("A284C2436C4B40E58451814C5275F7DF") },
                ActivityTypeIds = new List<Guid> { Guid.Parse("C14519AD26D7402998DDD0498C90318B"), Guid.Parse("C5C5167AC50B4526B9C1CD207B23CC75") },
                StartDate = new DateTime(2018, 1, 1),
                DueDate = DateTime.Now,
            });

            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var tableHeaders = tableHelper.GetTeamsByFiltersReportTableHeaders();

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetTeamsByFiltersReport(request, tableHeaders);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", $"attachment; filename=GetTeamsByFiltersReport.xlsx");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<ActionResult> GetGeneralReport(List<Guid> groupsIds, List<Guid> projectsIds, List<Guid> teamsIds, DateTime startDate, DateTime dueDate)
        {
            var request = await Mediator.Send(new GetGeneralReportQuery
            {
                GroupsIds = groupsIds,
                ProjectsIds = projectsIds,
                TeamsIds = teamsIds,
                StartDate = startDate,
                EndDate = dueDate
            });

            if (request == null) return BadRequest();

            return PartialView("_GeneralReport", request);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetHistoryReport()
        {
            var query = new GetProjectGroupsByFiltersQuery
            {
                AssigneeIds = new List<Guid>
                {
                    Guid.Parse("338E8954B9E24281AE33ADA59CCD4C25"), Guid.Parse("9525D234-DB98-4411-A69B-D130810D8138"),
                    Guid.Parse("A284C2436C4B40E58451814C5275F7DF"), Guid.Parse("5E4F55A6746D41BE9B3237E6E35D35A1")
                },
                ActivityTypeIds = new List<Guid>
                    {Guid.Parse("C14519AD26D7402998DDD0498C90318B"), Guid.Parse("C5C5167AC50B4526B9C1CD207B23CC75")},
                StartDate = new DateTime(2017, 4, 1),
                DueDate = DateTime.Now,
                ProjectStatuses = new List<ProjectStatus>
                    {ProjectStatus.Canceled, ProjectStatus.Completed, ProjectStatus.InProgress, ProjectStatus.New},
                IntervalType = Interval.Trimester
            };

            var request = await Mediator.Send(query);
            if (request == null) return BadRequest();

            var tableHelper = new ExcelTableHelper();
            var tableHeaders = tableHelper.GetProjectGroupsWithHistoryFilteredReportTableHeaders();
            tableHeaders[1].IsActive = false;

            var excelReportHelper = new ExcelReportHelper();
            var result = excelReportHelper.GetProjectGroupsFilteredReportWithHistory(request, query.StartDate, query.DueDate, query.IntervalType, tableHeaders);

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", $"attachment; filename=GetTeamsByFiltersReport.xlsx");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.ReportsManagement)]
        public async Task<IActionResult> DownloadReport(string fileName)
        {
            var result = (byte[])TempData["Download_reportByte"];

            if (result == null) return BadRequest();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", $"attachment; filename={fileName}");
            await Response.Body.WriteAsync(result);

            return Ok();
        }

        public async Task<IActionResult> ActivityTypeChart(Guid projectId)
        {
            var model = await Mediator.Send(new GetTasksByTypeReportQuery
            {
                ProjectId = projectId,
                ShowAllData = true
            });

            if (model == null)
                return BadRequest();

            return PartialView("Dashboards/_ActivityTypeChart", model);
        }

        public async Task<IActionResult> ActivityStatusChart(Guid projectId)
        {
            var model = await Mediator.Send(new GetTasksByStatusesReportQuery
            {
                ProjectId = projectId,
                ShowAllData = true
            });

            if (model == null)
                return BadRequest();

            return PartialView("Dashboards/_ActivityStatusChart", model);
        }

        public async Task<IActionResult> ActivityPriorityChart(Guid projectId)
        {
            var model = await Mediator.Send(new GetTasksByPriorityReportQuery
            {
                ProjectId = projectId,
                ShowAllData = true
            });

            if (model == null)
                return BadRequest();

            return PartialView("Dashboards/_ActivityPriorityChart", model);
        }

        public async Task<IActionResult> EmployeesTasksChart(Guid projectId)
        {
            var model = await Mediator.Send(new GetTasksByEmployeesReportQuery
            {
                ProjectId = projectId,
                ShowAllData = true
            });

            if (model == null)
                return BadRequest();

            return PartialView("Dashboards/_EmployeesTasksChart", model);
        }

        public async Task<IActionResult> ProjectOverallProgressChart(Guid projectId)
        {
            var model = await Mediator.Send(new GetProjectOverallProgressQuery {ProjectId = projectId});

            if (model == null)
                return BadRequest();

            return PartialView("Dashboards/_ProjectOverallProgressChart", model);
        }

        public async Task<IActionResult> ActivityTypeTrackerChart(Guid projectId, Guid activityTypeId)
        {
            var model = await Mediator.Send(new GetProjectActivityTypeTrackersQuery
            {
                ProjectId = projectId,
                ActivityTypeId = activityTypeId
            });

            if (model == null)
                return BadRequest();

            return PartialView("Dashboards/_ProjectActivityTypeTrackerChart", model);
        }
    }
}