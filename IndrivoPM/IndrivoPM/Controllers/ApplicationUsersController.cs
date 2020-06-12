using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gear.Common.Extensions.Result;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Identity.Manager.Domain.Roles.Queries.GetRoleList;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.ActivateApplicationUser;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.AddToRoles;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.CreateApplicationUser;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.DeleteApplicationUsers;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.UpdateApplicationUser;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserDetails;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;
using Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitList;
using Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetJobDepartmentTeamList;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList;
using Gear.Manager.Core.Infrastructure.MediatorResultHandler;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListByAssignee;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetPersonalActivitiesByProject;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByEmployee;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetAllProjects;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembers;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetUserProjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;
using Platform = Bizon360.Models.Platform;

namespace Bizon360.Controllers
{
    [Authorize]

    public class ApplicationUsersController : BaseController
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _autoMapper;
        private readonly IMediatrResultFactory _mediatorFactory;
        public ApplicationUsersController(UserManager<ApplicationUser> userManager, IMapper autoMapper, IMediatrResultFactory mediatorFactory)
        {
            _userManager = userManager;
            _autoMapper = autoMapper;
            _mediatorFactory = mediatorFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Adm;
        }

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        [HasPermission(Permissions.EmployeeRead)]
        public IActionResult Index()
        {
            return View();
        }

        [HasPermission(Permissions.EmployeeRead)]
        public async Task<IActionResult> GetEmployeeList()
        {
            var userList = await Mediator.Send(new GetApplicationUserListQuery());

            foreach (var item in userList.ApplicationUsers)
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString());
                item.Roles.AddRange(await _userManager.GetRolesAsync(user));  
            }

            return PartialView("_EmployeeList", userList);
        }

        [HasPermission(Permissions.EmployeeUpdate)]
        public async Task<IActionResult> ActivateApplicationUser(List<Guid> id)
        {
            await Mediator.Send(new ActivateApplicationUserCommand() {Ids = id, Active = true});
            return Ok();
        }

        [HasPermission(Permissions.EmployeeUpdate)]
        public async Task<IActionResult> DeactivateApplicationUser(List<Guid> id)
        {
            await Mediator.Send(new ActivateApplicationUserCommand() {Ids = id, Active = false});
            return Ok();
        }


        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.EmployeeCreate)]
        public async Task<IActionResult> Create()
        {
            var departments = Mediator.Send(new GetDepartmentListQuery()).Result.Departments
                .Where(x => x.IsDeletable).ToList();
            var teams = Mediator.Send(new GetDepartmentTeamListQuery()).Result.DepartmentTeams.ToList();
            var roles = await Mediator.Send(new GetRoleListQuery());
            var projectsDto = await Mediator.Send(new GetAllProjectsQuery());

            ViewBag.Departments = departments.Count == 0 ? null : departments;
            ViewBag.Teams = teams.Count == 0 ? null : teams;
            ViewBag.Roles = roles.Count == 0 ? null : roles.Where(x => x.Active);
            ViewBag.Projects = projectsDto.Projects.Count == 0 ? null : projectsDto.Projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.EmployeeCreate)]
        public async Task<ActionResult> Create(CreateApplicationUserCommand command)
        {
            Result createEmployeeCommand = await _mediatorFactory.Execute(command);

            if (createEmployeeCommand.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return BadRequest(createEmployeeCommand.Error);

        }

        // Todo: need to refactor
        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.EmployeeUpdate)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();
            var request = await Mediator.Send(new GetApplicationUserDetailQuery {Id = id.Value});

            if (request == null)
                return BadRequest();

            // Get selected teams
            var selectedTeams = new List<DepartmentTeamLookupModel>();

            foreach (var item in request.Teams)
                selectedTeams.Add(Mediator.Send(new GetDepartmentTeamListQuery()).Result.DepartmentTeams
                    .First(x => x.Id == item.Key));

            var departments = Mediator.Send(new GetDepartmentListQuery()).Result.Departments
                .Where(x => x.IsDeletable).ToList();

            ViewBag.Departments = departments.Count == 0 ? null : departments;
            
            var businessUnits = Mediator.Send(new GetBusinessUnitListQuery()).Result.BusinessUnits.Where(x => x.Active).ToList();

            ViewBag.BusinessUnits = businessUnits.Count == 0 ? null : businessUnits;

            //// Team list form department
            //var teams = new List<DepartmentTeamLookupModel>();
            //foreach (var item in selectedTeams)
            //    if (teams.Any(x => x.DepartmentId == item.DepartmentId) == false)
            //        teams.AddRange(Mediator.Send(new GetDepartmentTeamListQuery()).Result.DepartmentTeams
            //            .Where(x => x.DepartmentId == item.DepartmentId).ToList());
            var teams = Mediator.Send(new GetDepartmentTeamListQuery()).Result.DepartmentTeams.ToList();
            ViewBag.Teams = teams.Count == 0 ? null : teams;

            // Job list from selected team
            var jobsFromTeams = new List<JobPositionLookupModel>();
            foreach (var item in selectedTeams)
            foreach (var job in item.JobPositions)
                jobsFromTeams.AddRange(Mediator.Send(new GetJobPositionListQuery()).Result.JobPositions
                    .Where(x => x.Name.Contains(job.Name)));

            ViewBag.Jobs = jobsFromTeams;

            // Role list
            var roles = await Mediator.Send(new GetRoleListQuery());

            ViewBag.Roles = roles.Count == 0 ? null : roles.Where(x => x.Active);
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            request.RoleNames = await _userManager.GetRolesAsync(user);

            var projectsDto = await Mediator.Send(new GetAllProjectsQuery());
            ViewBag.Projects = projectsDto.Projects.Count == 0 ? null : projectsDto.Projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            return View(Mapper.Map<UpdateApplicationUserCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.EmployeeUpdate)]
        public async Task<ActionResult> Edit(UpdateApplicationUserCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            // call update user roles:
            var rolesCommand = new AddToRolesCommand();
            rolesCommand.UserId = command.Id;
            rolesCommand.RoleNames = command.RoleNames;

            if (await Mediator.Send(rolesCommand) == null)
                return BadRequest();


            return RedirectToAction("Details", new { id = command.Id });
        }

        [HttpPost]
        [HasPermission(Permissions.EmployeeDelete)]
        public async Task<IActionResult> DeleteEmployees(List<Guid> entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            Result deleteCommand = await _mediatorFactory.Execute(new DeleteApplicationUsersCommand { UsersById = entities });
            
            if (deleteCommand.IsSuccess)
            {
                return Json(new { deletedRows = entities });
            }
            return BadRequest(deleteCommand.Error);
        }

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var request = await Mediator.Send(new GetApplicationUserDetailQuery { Id = id });

            if (request == null) return BadRequest();

            var u = await _userManager.FindByIdAsync(request.Id.ToString());
            request.RoleNames = await _userManager.GetRolesAsync(u);

            var projectsDto = await Mediator.Send(new GetUserProjectsQuery { UserId = id });

            ViewBag.Projects = projectsDto.Projects.Count == 0 ? null : projectsDto.Projects;

            #region Comment
            /*

                        var departments = new List<DepartmentLookupModel>();
                        foreach (var item in request.Teams)
                        {
                            if (departments.Any(s => s.Id == item.DepartmentId)) continue;
                            departments.Add(Mediator.Send(new GetDepartmentListQuery())
                                .Result.Departments.First(x => x.Id == item.DepartmentId));
                        }

                        request.Departments = departments.Where(x => x.IsDeletable).ToDictionary(x => x.Id, x => x.Name);


                        var businessUnits = new List<BusinessUnitLookupModel>();
                        if (departments.Count != 0)
                        {

                            foreach (var item in departments)
                            {
                                if (businessUnits.Any(x => x.Id == item.BusinessUnitId) || item.BusinessUnitId == null) continue;
                                businessUnits.Add(Mediator.Send(new GetBusinessUnitListQuery())
                                    .Result.BusinessUnits.First(x => x.Id == item.BusinessUnitId));
                            }
                        }

                        request.BusinessUnits = businessUnits.Where(x => x.IsDeletable).ToDictionary(x => x.Id, x => x.Name);
            var detailViewModel = _autoMapper.Map<ApplicationUserDetailModel, ApplicationUserDetailViewModel>(request);*/
            #endregion

            return View(request);
        }
        [HttpGet]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> GetUserProjects(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var request = await Mediator.Send(new GetUserProjectsQuery { UserId = id });
            if (request != null)
            {
                return PartialView("_UserProjects", request);
            }

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamRead)]
        public JsonResult GetTeamsList(List<Guid> departmentId)
        {
            var teams = new List<DepartmentTeamLookupModel>();

            foreach (var item in departmentId)
            {
                teams.AddRange(Mediator.Send(new GetDepartmentTeamListQuery()).Result.DepartmentTeams.Where(x => x.DepartmentId == item).ToList());
            }

            var test = Mediator.Send(new GetJobPositionListQuery()).Result.JobPositions.ToList();

            return Json(new SelectList(teams, "Id", "Name"));
        }

        [HttpPost]
        [HasPermission(Permissions.JobPositionRead)]
        public  JsonResult GetJobList(List<Guid> teamIds)
        {
            var jobs = Mediator.Send(new GetJobDepartmentTeamListQuery(){TeamIds = teamIds}).Result.JobPositions.ToList();
            return Json(new SelectList(jobs, "Id", "Name"));
        }

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> GetUserActivities(Guid id)
        {
            var request = await Mediator.Send(new GetPersonalActivitiesByProjectQuery
            {
                UserId = id,
            });

            if (request != null)
            {
                ViewBag.UserId = id;
                
                var user = await _userManager.FindByIdAsync(id.ToString());
                ViewBag.FullName = $"{user.FirstName} {user.LastName}";
                return View(request);
            }

            return BadRequest();
        }


        [HttpGet]
        [HasPermission(Permissions.RoleAssign)]
        public async Task<ActionResult> AddToRoles(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var roles = await Mediator.Send(new GetRoleListQuery());

            ViewBag.Roles = roles.Count == 0 ? null : roles.Where(x => x.Active);

            var user = await _userManager.FindByIdAsync(id.ToString());

            var vm = new AddToRolesCommand
            {
                UserId = user.Id,
                RoleNames = await _userManager.GetRolesAsync(user)
            };

            return PartialView("_AddtoRoles", vm);
        }

        [HttpPost]
        [HasPermission(Permissions.RoleAssign)]
        public async Task<ActionResult> AddToRoles(AddToRolesCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> GetActivitiesByAssignee(Guid id, string[] openedCollapses, string searchValue)
        {
            var request = await Mediator.Send(new GetActivityListByAssigneeQuery
            {
                ProjectId = id,
                OpenedCollapsesById = openedCollapses
            });

            if (request == null) return BadRequest();

            ViewBag.ProjectId = id;
            ViewBag.HasResults = false;

            if (string.IsNullOrEmpty(searchValue))
            {
                ViewBag.Searching = false;
                ViewBag.HasResults = true;
                return PartialView("_ActivitiesByAssignee", request);
            }

            ViewBag.Searching = true;
            foreach (var activityList in request.Assignees)
            {
                activityList.Activities = activityList.Activities
                    .Where(a => a.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                if (!ViewBag.HasResults && activityList.Activities.Any())
                    ViewBag.HasResults = true;
            }

            return PartialView("_ActivitiesByAssignee", request);
        }

        #region Activities view

        public async Task<IActionResult> GetProjectMembers(Guid projectId, ICollection<ActivityStatus> filter)
        {
            if (projectId.Equals(Guid.Empty)) return UnprocessableEntity();

            var request = await Mediator.Send(new GetProjectMembersQuery { Id = projectId, Filter = filter });

            if (request == null) return BadRequest();

            ViewBag.ProjectId = projectId;

            return PartialView("_ProjectMembers", request);
        }

        public async Task<IActionResult> SearchActivitiesByEmployee(Guid projectId, string searchValue)
        {
            if (projectId.Equals(Guid.Empty) || string.IsNullOrEmpty(searchValue)) return UnprocessableEntity();

            var request = await Mediator.Send(new SearchActivitiesByEmployeeQuery
                {ProjectId = projectId, SearchValue = searchValue});

            if (request != null)
            {
                ViewBag.ProjectId = projectId;

                return PartialView("_EmployeeSearchResult", request);
            }

            return BadRequest();
        }

        #endregion
    }
}