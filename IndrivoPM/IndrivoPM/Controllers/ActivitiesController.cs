using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bizon360.Models;
using Bizon360.Utils;
using Bizon360.ViewComponents;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.Common.Extensions.Result;
using Gear.Common.Extensions.StringExtensions;
using Gear.Domain.PmEntities.Enums;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Identity.Permissions.Infrastructure.Utils;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.DeleteFile;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.RefreshUserToken;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.UploadFile;
using Gear.Manager.Core.EntityServices.CloudStorage.Querries.CheckUserToken;
using Gear.Manager.Core.EntityServices.CloudStorage.Querries.GetChildren;
using Gear.Manager.Core.Infrastructure.MediatorResultHandler;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.AssignActivitiesToSprint;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.DeleteActivities;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.MarkActivitiesAsCompleted;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.MoveActivitiesToList;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.RemoveActivitiesFromSprint;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.SetActivitiesPriority;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.SetActivitiesStatus;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.CreateActivity;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.DeleteActivity;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.MoveToList;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.MoveToSprint;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.RemoveFromSprint;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.RenameActivity;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.UpdatePriority;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.UpdateStatus;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateActivity;
using Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateOrderActivities;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivitiesForGrid;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityAudit;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityOverview;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByName;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchAllActivities;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityLists;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityListsDto;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeList;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypesDto;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetProjectActivityTypes;
using Gear.ProjectManagement.Manager.Domain.Assignees.Commands.EditActivityAssignees;
using Gear.ProjectManagement.Manager.Domain.Assignees.Queries.GetActivityAssignees;
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestDetails;
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetRequestName;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectList;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembers;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembersDto;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectName;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsDto;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectTabsNav;
using Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintDetailQuery;
using Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintListQuery;
using Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    [Authorize]
    public class ActivitiesController : BaseController
    {
        private readonly IMediatrResultFactory _mediatorFactory;

        public ActivitiesController(IMediatrResultFactory mediatorFactory)
        {
            _mediatorFactory = mediatorFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Pm;
        }

        #region Activities

        [HttpGet]
        [Route("{controller}/{id}")]
        [Breadcrumb("ViewData.SecondNode", FromController = typeof(ProjectsController), FromAction = "Index")]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> Index(Guid id, ActivitiesView view = ActivitiesView.Grid)
        {
            var isUserPm = User.UserHasThisPermission(Permissions.ProjectUpdate);

            var request = await Mediator.Send(new GetProjectTabsNavCommand { Id = id, IsUserPm = isUserPm });

            if (request == null) return Unauthorized();

            ViewBag.SprintItems = GetSprintSelectList(id);

            ViewBag.CurrentView = view;

            return View(request);
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<ActionResult> ActivitiesUpdateOrder(List<Guid> itemIds)
        {
            var command = new UpdateOrderActivitiesCommand { ActivitiesIds = itemIds };

            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult(true);
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> GetActivities(Guid projectId, List<Guid> sprintIds)
        {
            if (projectId.Equals(Guid.Empty)) return UnprocessableEntity();

            var request = await Mediator.Send(new GetActivitiesForGridQuery { ProjectId = projectId, SprintIds = sprintIds });

            if (request == null) return BadRequest();

            ViewBag.ProjectId = projectId;

            ViewBag.SprintIds = sprintIds;

            return PartialView("_Grid", request);
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> GetActivitiesFromParentEntity(ActivityParentType parentType, Guid parentId, Guid projectId, ICollection<ActivityStatus> filter, int skip = 0, int size = 10)
        {
            if (parentType.Equals(ActivityParentType.None)) return BadRequest();

            if (parentId.Equals(Guid.Empty)) return UnprocessableEntity();


            var request = await Mediator.Send(new GetActivityListFromParentEntityQuery
            {
                ParentType = parentType,
                ParentEntityId = parentId,
                Filter = filter,
                Skip = skip,
                Size = size,
                ProjectId = projectId
            });

            if (request == null) return BadRequest();

            ViewBag.ParentType = parentType;
            ViewBag.ParentId = parentId;

            return PartialView(skip > 0 ? "_ActivityTableRows" : "_Activities", request);

        }

        [HttpGet]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> SearchAllActivities(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue)) return NotFound();

            var request = await Mediator.Send(new SearchAllActivitiesQuery { SearchValue = searchValue });

            if (request != null)
            {
                return PartialView("_GlobalSearchResult", request);
            }

            return BadRequest();
        }

        [HttpGet]
        [Breadcrumb("ViewData.Title", FromController = typeof(ProjectsController), FromAction = "Details")]
        [HasPermission(Permissions.ActivityCreate)]
        public IActionResult Create(Guid? projectId, string dtoJson)
        {
            var dto = !string.IsNullOrEmpty(dtoJson) ? JsonConvert.DeserializeObject<ActivityDto>(dtoJson) : new ActivityDto();

            // Breadcrumb data
            ViewData["SecondNode"] = projectId.HasValue ? Mediator.Send(new GetProjectNameQuery {Id = projectId.Value}).Result : "No project";
            // Used to append project id to the parent breadcrumb node
            ViewBag.LinkToReplace = Url.Action("Details", "Projects");
            ViewBag.BreadcrumbLink = Url.Action("Index", "Activities", new { id = projectId });

            var getAllProjects = User.UserHasThisPermission(Permissions.ProjectUpdate);
            ViewBag.Projects = Mediator.Send(new GetProjectsDtoQuery { GetAllProjects = getAllProjects }).Result;
            ViewBag.ActivityTypes = GetEnabledProjectActivityTypeSelectList(projectId.Value);

            if (projectId.HasValue && projectId.Value != Guid.Empty)
            {
                ViewBag.ApplicationUsers = Mediator.Send(new GetProjectMembersDtoQuery { Id = projectId.Value }).Result;
                ViewBag.ActivityLists = Mediator.Send(new GetActivityListsDtoQuery { ProjectId = projectId.Value }).Result;
                ViewBag.Sprints = Mediator.Send(new GetSprintsDtoQuery { ProjectId = projectId.Value }).Result;
                ViewBag.ReturnId = projectId;

                return View(new CreateActivityCommand
                {
                    Name = string.IsNullOrEmpty(dto.Name) ? string.Empty : dto.Name,
                    Description = string.IsNullOrEmpty(dto.Description) ? string.Empty : dto.Description,
                    ProjectId = projectId.Value,
                    StartDate = DateTime.Now,
                    DueDate = dto.SprintEndDate,
                    Priority = dto.Priority
                });
            }

            return View();

        }
        
        [HttpPost]
        [Breadcrumb("ViewData.Title", FromController = typeof(ProjectsController), FromAction = "Details")]
        [HasPermission(Permissions.ActivityCreate)]
        public async Task<IActionResult> CreateActivity(string requestName, Guid requestId, Guid? projectId, string projectName, string description, ActivityPriority priority, IFormFile file)
        {
            if (file != null)
                await UploadFilesForRequest(projectName, projectId, requestName, requestId, file);

            // Breadcrumb data
            ViewData["SecondNode"] = projectId.HasValue ? Mediator.Send(new GetProjectNameQuery { Id = projectId.Value }).Result : "No project";
            // Used to append project id to the parent breadcrumb node
            ViewBag.LinkToReplace = Url.Action("Details", "Projects");
            ViewBag.BreadcrumbLink = Url.Action("Index", "Activities", new { id = projectId });

            var getAllProjects = User.UserHasThisPermission(Permissions.ProjectUpdate);
            ViewBag.Projects = Mediator.Send(new GetProjectsDtoQuery { GetAllProjects = getAllProjects }).Result;
            ViewBag.ActivityTypes = GetEnabledProjectActivityTypeSelectList(projectId.Value);

            if (projectId.HasValue && projectId.Value != Guid.Empty)
            {
                ViewBag.ApplicationUsers = Mediator.Send(new GetProjectMembersDtoQuery { Id = projectId.Value }).Result;
                ViewBag.ActivityLists = Mediator.Send(new GetActivityListsDtoQuery { ProjectId = projectId.Value }).Result;
                ViewBag.Sprints = Mediator.Send(new GetSprintsDtoQuery { ProjectId = projectId.Value }).Result;
                ViewBag.ReturnId = projectId;

                return View("Create", new CreateActivityCommand
                {
                    Name = string.IsNullOrEmpty(requestName) ? string.Empty : requestName,
                    Description = string.IsNullOrEmpty(description) ? string.Empty : description,
                    ProjectId = projectId.Value,
                    StartDate = DateTime.Now,
                    Priority = priority,
                    ChangeRequestId = requestId
                });
            }

            return View("Create");

        }

        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> UploadFilesForRequest(string projectName, Guid? projectId, string changeRequestName, Guid entityId, IFormFile file)
        {
            await Mediator.Send(new RefreshCurrentUserTokenCommand { ExternalProvider = ExternalProviders.OneDrive });

            if (file.Length <= 60 * 1024 * 1024)
            {
                var command = new UploadFileCommand();
                command.ExternalProvider = ExternalProviders.OneDrive;
                command.Filepath = "drive/special/approot:/PM/" + "Projects/" + projectName + projectId.Value + "/" +
                                   "Requests/" + (changeRequestName?.ReplaceSpecialCharacters('_') ?? string.Empty) +
                                   entityId;
                command.FormFile = file;

                if (await Mediator.Send(command) != null)
                    return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityCreate)]
        public IActionResult CreateByStatus(Guid projectId, ActivityStatus status)
        {
            var model = new CreateActivityCommand { Status = status };

            ViewBag.ApplicationUsers = Mediator.Send(new GetProjectMembersDtoQuery { Id = projectId }).Result;
            ViewBag.ActivityTypes = GetEnabledProjectActivityTypeSelectList(projectId);
            ViewBag.ActivityLists = Mediator.Send(new GetActivityListsDtoQuery { ProjectId = projectId }).Result;
            ViewBag.Sprints = Mediator.Send(new GetSprintsDtoQuery { ProjectId = projectId }).Result;

            return PartialView("_CreateByStatus", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityCreate)]
        public async Task<IActionResult> Create(CreateActivityCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                return RedirectToAction("Details", "Activities", new { id = command.Id });
            }
               

            return View(command);
        }

        [HttpGet]
        [Breadcrumb("ViewData.Title", FromController = typeof(ProjectsController), FromAction = "Details")]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id.Equals(Guid.Empty)) return UnprocessableEntity();
            
            var request = await Mediator.Send(new GetActivityDetailQuery { Id = id });
            if (request != null)
            {
                // Breadcrumb data
                ViewData["SecondNode"] = request.ProjectName;
                // Used to append project id to the parent breadcrumb node
                ViewBag.LinkToReplace = Url.Action("Details", "Projects");
                ViewBag.BreadcrumbLink = Url.Action("Index", "Activities", new { id = request.ProjectId });

                var getAllProjects = User.UserHasThisPermission(Permissions.ProjectUpdate);
                ViewBag.Projects = Mediator.Send(new GetProjectsDtoQuery { GetAllProjects = getAllProjects }).Result;
                ViewBag.ActivityTypes = GetEnabledProjectActivityTypeSelectList(request.ProjectId);
                ViewBag.ApplicationUsers = Mediator.Send(new GetProjectMembersDtoQuery { Id = request.ProjectId }).Result;
                ViewBag.ActivityLists = Mediator.Send(new GetActivityListsDtoQuery { ProjectId = request.ProjectId }).Result;
                ViewBag.Sprints = Mediator.Send(new GetSprintsDtoQuery { ProjectId = request.ProjectId }).Result;



                ViewBag.ReturnId = request.ProjectId;

                return View(Mapper.Map<UpdateActivityCommand>(request));
            }

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> Edit(UpdateActivityCommand command)
        {
            if (await Mediator.Send(command) != null)
                return RedirectToAction("Details", "Activities", new { id = command.Id });

            return View(command);
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> EditActivityAssignees(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetActivityAssigneesQuery { ActivityId = id.Value });
            if (request != null)
                return PartialView("_EditActivityAssignees",
                    new EditActivityAssigneesCommand
                    {
                        Id = id.Value,
                        Users = request.Assignees.Select(x => x.Id).ToList()
                    });

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> EditActivityAssignees(EditActivityAssigneesCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                var request = await Mediator.Send(new GetActivityAssigneesQuery { ActivityId = command.Id });

                if (request != null)
                {
                    ViewBag.ActivityId = command.Id;
                    return Json(new
                    {
                        id = command.Id,
                        view = await this.RenderViewAsync("_ActivityAssignees", request, true)
                    });
                }

                return Ok();
            }

            return BadRequest();
        }



        [HttpGet]
        [Breadcrumb("ViewData.Title", FromController = typeof(ProjectsController), FromAction = "Details")]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetActivityDetailQuery() { Id = id.Value, DisplayAuditInfo = false });
            if (request != null)
            {
                // Used to append project id to the parent breadcrumb node
                ViewBag.LinkToReplace = Url.Action("Details", "Projects");
                ViewBag.BreadcrumbLink = Url.Action("Index", "Activities", new { id = request.ProjectId });
                ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());

                return View(request);
            }

            return BadRequest();
        }
        [HttpGet]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> GetOverview(Guid? activityId)
        {
            if (!activityId.HasValue || activityId.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetActivityOverviewQuery() { Id = activityId.Value });
            if (request != null)
            {
                return Json(new { loggedTime = request.LoggedTime, status = request.Status.ToString(), progress = request.Progress });
            }

            return BadRequest();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityDelete)]
        public async Task<IActionResult> Delete(Guid? id, Guid? projectId)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            if (await Mediator.Send(new DeleteActivityCommand {Id = id.Value}) != null)
            {
                if (projectId.HasValue && projectId != Guid.Empty)
                    return RedirectToAction("Index", "Activities", new {id = projectId.Value});

                return Json(new { id = id.Value });
            }

            return BadRequest();
        }

        public async Task<IActionResult> GetAudit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var model = await Mediator.Send(new GetActivityAuditQuery {Id = id});
            if (model != null)
            {
                return PartialView("_ActivityAudit", model);
            }

            return BadRequest();
        }

        #endregion
        
        #region Shortcut Actions

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public IActionResult EditPriority(Guid? id, ActivityPriority? priority, bool bulk)
        {
            ViewBag.Bulk = bulk;
            return PartialView("_EditPriority");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> EditPriority(UpdatePriorityCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new
                {
                    priority = command.Priority.ToString(),
                    id = command.Id
                });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public IActionResult EditStatus(Guid? id, ActivityStatus? status)
        {
            return PartialView("_EditStatus");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> EditStatus(UpdateStatusCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new
                {
                    id = command.Id,
                    status = command.Status.ToString()
                });

            return BadRequest();
        }
        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> UpdateActivityStatus([FromBody] UpdateStatusCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new
                {
                    id = command.Id,
                    status = command.Status.ToString()
                });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public IActionResult MoveToList(Guid? projectId, Guid? id, Guid? activityListId)
        {
            if (projectId.HasValue && projectId.Value != Guid.Empty)
            {
                ViewBag.ActivityLists = Mediator.Send(new GetActivityListsDtoQuery {ProjectId = projectId.Value}).Result;
            }

            ViewBag.OldListId = activityListId;
            return PartialView("_MoveToList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> MoveToList(MoveToListCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new
                {
                    id = command.Id,
                    activityListId = command.ActivityListId
                });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public IActionResult AssignToSprint(Guid? projectId, Guid? id, Guid? sprintId)
        {
            if (projectId.HasValue && projectId.Value != Guid.Empty)
            {
                ViewBag.Sprints = Mediator.Send(new GetSprintsDtoQuery {ProjectId = projectId.Value}).Result;
            }

            ViewBag.OldSprintId = sprintId;
            return PartialView("_AssignToSprint");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> AssignToSprint(MoveToSprintCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            var newDate = string.Empty;
            var sprintName = string.Empty;
            var sprintDetails = await Mediator.Send(new GetSprintDetailQuery { Id = command.SprintId });
            if (sprintDetails != null)
            {
                newDate = $"{sprintDetails.EndDate:yyyy-MM-dd}";
                sprintName = sprintDetails.Name;
            }
                
            return Json(new
            {
                id = command.Id,
                sprintId = command.SprintId,
                date = newDate,
                sprintName = sprintName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> RemoveFromSprint(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            if (await Mediator.Send(new RemoveFromSprintCommand { Id = id.Value }) != null)
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public IActionResult Rename(Guid? id, string name)
        {
            return PartialView("_Rename");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> Rename(RenameActivityCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { id = command.Id, name = command.Name });

            return BadRequest();
        }

        #endregion

        #region SelectList Methods

        private List<SelectListItem> GetProjectSelectList()
        {
            var getAllProjects = User.UserHasThisPermission(Permissions.ProjectUpdate);
            var request = Mediator.Send(new GetProjectListQuery { GetAllProjects = getAllProjects }).Result;
            return request?.Projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).Prepend(new SelectListItem
            {
                Value = string.Empty,
                Text = "- Select -",
                Selected = true
            }).ToList();
        }

        private List<SelectListItem> GetEmployeeSelectList(Guid? projectId)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return new List<SelectListItem>();


            var request = Mediator.Send(new GetProjectMembersQuery { Id = projectId.Value }).Result;
            if (request != null)
            {
                return request.Users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FullName
                }).ToList();
            }

            return new List<SelectListItem>();
        }

        private List<SelectListItem> GetActivityTypeSelectList(Guid projectId)
        {
            var activityTypes = Mediator.Send(new GetProjectActivityTypesQuery { ProjectId = projectId }).Result;
            return activityTypes?.ProjectActivityTypes.Select(p => new SelectListItem
            {
                Value = p.ActivityTypeId.ToString(),
                Text = p.ActivityTypeName,
                Selected = p.Active
            }).Prepend(new SelectListItem
            {
                Value = string.Empty,
                Text = "- Select -",
                Selected = true
            }).ToList();
        }

        private List<SelectListItem> GetEnabledProjectActivityTypeSelectList(Guid projectId)
        {
            var activityTypes = Mediator.Send(new GetProjectActivityTypesQuery { ProjectId = projectId }).Result;
            return activityTypes?.ProjectActivityTypes.Where(x => x.Active).Select(p => new SelectListItem
            {
                Value = p.ActivityTypeId.ToString(),
                Text = p.ActivityTypeName
            }).ToList();
        }

        private List<SelectListItem> GetActivityListSelectList(Guid? projectId)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return new List<SelectListItem>();

            var request = Mediator.Send(new GetActivityListsQuery { ProjectId = projectId.Value }).Result;
            if (request != null)
            {
                return request.ActivityLists.Select(al => new SelectListItem
                {
                    Value = al.Id.ToString(),
                    Text = al.Name
                }).Prepend(new SelectListItem
                {
                    Value = string.Empty,
                    Text = "- Select -",
                    Selected = true
                }).ToList();
            }

            return new List<SelectListItem>();
        }

        private List<SelectListItem> GetSprintSelectList(Guid? projectId)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return new List<SelectListItem>();

            var request = Mediator.Send(new GetSprintListQuery { ProjectId = projectId.Value }).Result;
            if (request != null)
            {
                return request.Sprints.Where(x => !x.IsCompleted).Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).Prepend(new SelectListItem
                {
                    Value = string.Empty,
                    Text = "- Select -",
                    Selected = true
                }).ToList();
            }

            return new List<SelectListItem>();
        }

        [HttpGet]
        public async Task<IActionResult> GetSprintListByProject(Guid projectId)
        {
            if (projectId.Equals(Guid.Empty)) return UnprocessableEntity();

            var request = await Mediator.Send(new GetSprintListQuery { ProjectId = projectId });
            if (request != null)
            {
                return Json(request.Sprints.Where(s => !s.IsCompleted).Select(s => new
                {
                    value = s.Id.ToString(),
                    text = s.Name
                }).Prepend(new
                {
                    value = string.Empty,
                    text = "- Select -"
                }));
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberListByProject(Guid? projectId)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetProjectMembersQuery { Id = projectId.Value });
            if (request != null)
            {
                return Json(request.Users.Select(u => new
                {
                    value = u.Id.ToString(),
                    text = u.FullName
                }).Prepend(new
                {
                    value = string.Empty,
                    text = "- Select -"
                }).ToList());
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityListRead)]
        public async Task<IActionResult> GetActivityListsByProject(Guid? projectId)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetActivityListsQuery { ProjectId = projectId.Value });
            if (request != null)
            {
                return Json(request.ActivityLists.Select(al => new
                {
                    value = al.Id.ToString(),
                    text = al.Name
                }).Prepend(new
                {
                    value = string.Empty,
                    text = "- Select -"
                }).ToList());
            }

            return BadRequest();
        }

        #endregion

        #region Bulk Actions

        [HttpPost]
        [HasPermission(Permissions.ActivityDelete)]
        public async Task<IActionResult> DeleteActivities(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            if (await Mediator.Send(new DeleteActivitiesCommand {ActivitiesById = entities.ToList()}) != null)
                return Json(new { deletedRows = entities });

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> RemoveActivitiesFromSprint(Guid[] ids)
        {
            if (!ids.Any()) return UnprocessableEntity();

            if (await Mediator.Send(new RemoveActivitiesFromSprintCommand {ActivitiesById = ids.ToList()}) != null)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> MarkActivitiesAsCompleted(Guid[] ids)
        {
            if (!ids.Any()) return UnprocessableEntity();

            if (await Mediator.Send(new MarkActivitiesAsCompletedCommand {ActivitiesById = ids.ToList()}) != null)
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public IActionResult SetActivitiesPriority(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            return PartialView("_BulkSetPriority", new SetActivitiesPriorityCommand { ActivitiesById = entities });
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> SetActivitiesPriority(SetActivitiesPriorityCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { activities = command.ActivitiesById, priority = command.Priority.ToString() });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public IActionResult SetActivitiesStatus(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            return PartialView("_BulkUpdateStatus", new SetActivitiesStatusCommand { ActivitiesById = entities });
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> SetActivitiesStatus(SetActivitiesStatusCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { activities = command.ActivitiesById, status = command.Status.ToString() });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public IActionResult AssignActivitiesToSprint(Guid[] entities, Guid projectId)
        {
            if (!entities.Any()) return UnprocessableEntity();

            var sprints = Mediator.Send(new GetSprintListQuery { ProjectId = projectId }).Result;
            if (sprints != null)
            {
                ViewBag.Sprints = sprints.Sprints.Where(s => !s.IsCompleted).Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList();
            }

            return PartialView("_BulkAssignToSprint", new AssignActivitiesToSprintCommand { ActivitiesById = entities });
        }
        
        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> AssignActivitiesToSprint(AssignActivitiesToSprintCommand command)
        {
            var newDate = string.Empty;
            var sprintName = string.Empty;
            var sprintDetails = await Mediator.Send(new GetSprintDetailQuery { Id = command.SprintId });
            if (sprintDetails != null)
            {
                newDate = $"{sprintDetails.EndDate:yyyy-MM-dd}";
                sprintName = sprintDetails.Name;
            }

            if (await Mediator.Send(command) != null)
            {
                return Json(new
                {
                    activities = command.ActivitiesById,
                    sprintId = command.SprintId,
                    sprintName = sprintName,
                    date = newDate
                });
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityUpdate)]
        public IActionResult MoveActivitiesToList(Guid[] entities, Guid projectId)
        {
            if (!entities.Any()) return UnprocessableEntity();

            var activityLists = Mediator.Send(new GetActivityListsQuery {ProjectId = projectId}).Result;
            if (activityLists != null)
            {
                ViewBag.ActivityLists = activityLists.ActivityLists.Select(al => new SelectListItem
                {
                    Value = al.Id.ToString(),
                    Text = al.Name
                }).ToList();
            }

            return PartialView("_BulkMoveToList", new MoveActivitiesToListCommand { ActivitiesById = entities });
        }
            
        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> MoveActivitiesToList(MoveActivitiesToListCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                return Json(new { activities = command.ActivitiesById, activityListId = command.ActivityListId });
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> SearchActivityByName(Guid projectId, string searchValue)
        {
            if (projectId.Equals(Guid.Empty) || string.IsNullOrEmpty(searchValue)) return UnprocessableEntity();

            var request = await Mediator.Send(new SearchActivitiesByNameQuery
                { ProjectId = projectId, SearchValue = searchValue });

            if (request != null)
            {
                ViewBag.ProjectId = projectId;

                return PartialView("_Grid", request);
            }

            return BadRequest();
        }

        #endregion

        #region Cloud & Api

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> Files(Guid id)
        {
            var request = await Mediator.Send(new GetActivityDetailQuery { Id = id });
            if (request != null) return View(request);

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> UploadFile(int projectNumber, int activityNumber, IFormFile file)
        {
            var fileSize = 60 * 1024 * 1024;

            if (file.Length > fileSize) return BadRequest();

            Result refreshUserToken = await _mediatorFactory.Execute(new RefreshCurrentUserTokenCommand { ExternalProvider = ExternalProviders.OneDrive });

            if (!refreshUserToken.IsSuccess) return BadRequest();

            HttpResponseMessage uploadFileResult = await Mediator.Send(new UploadFileCommand
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Filepath = "drive/special/approot:/PM/" + "Projects/" + $"P{projectNumber.ToString().PadLeft(8, '0')}" + "/Activities/" + $"A{activityNumber.ToString().PadLeft(8, '0')}",
                FormFile = file
            });

            if (uploadFileResult.IsSuccessStatusCode)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> DeleteFile(string id, IList<string> paths, string fileName)
        {
            await Mediator.Send(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            try
            {
                var hasOperationCompletedWithSuccess = false;

                foreach (var path in paths)
                {
                    var request = new DeleteFileCommand
                    {
                        Id = id,
                        ExternalProvider = ExternalProviders.OneDrive,
                        FilePath = path,
                        FileName = fileName
                    };
                    hasOperationCompletedWithSuccess |= (await Mediator.Send(request) != null);
                }
                if (hasOperationCompletedWithSuccess)
                    return Ok(id);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Catch this: " + e.Message);
            }

            return BadRequest();
        }


        [HttpGet]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> GetFiles(string projectName, Guid? projectId, string activityName, Guid? activityId, Guid? changeRequestId, int projectNumber, int activityNumber)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty || !activityId.HasValue
                || activityId.Value == Guid.Empty) return BadRequest();

            Result refreshUserToken = await _mediatorFactory.Execute(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            var dto = new OneDriveTabDto()
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Paths = new List<string>
                {
                    "drive/special/approot:/PM/" + "Projects/" + projectName + projectId.Value + "/"
                    + "Activities/" + activityName + activityId.Value,
                    "drive/special/approot:/PM/" + "Projects/" + $"P{projectNumber.ToString().PadLeft(8, '0')}" + "/"
                       + "Activities/" + $"A{activityNumber.ToString().PadLeft(8, '0')}"},
                FolderName = "   "
            };

            if (refreshUserToken.IsSuccess)
            {
                var model = new OneDriveListViewModel()
                {
                    Paths = dto.Paths,
                    FolderName = dto.FolderName,
                    MetaData = new List<CloudMetaData>()
                };

                try
                {
                    model.MetaData = new List<CloudMetaData>();

                    if (changeRequestId.HasValue && changeRequestId != Guid.Empty)
                    {
                        var requestName = Mediator.Send(new GetRequestNameQuery {Id = changeRequestId.Value}).Result;
                        dto.Paths.Add("drive/special/approot:/PM/" + "Projects/" + projectName + projectId.Value + "/"
                                      + "Requests/" + requestName + changeRequestId.Value);

                        var changeRequest = await Mediator.Send(new GetChangeRequestDetailsQuery { Id = changeRequestId.Value });
                        dto.Paths.Add("drive/special/approot:/PM/" + "Projects/" + $"P{projectNumber.ToString().PadLeft(8, '0')}/" + "Requests/" + $"R{changeRequest.Number.ToString().PadLeft(8, '0')}");
                    }

                    foreach (var path in dto.Paths)
                    {
                        model.MetaData.AddRange(await Mediator.Send(new GetChildrenQuery()
                            {ExternalProvider = dto.ExternalProvider, FilePath = path }));
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Catch this: " + e.Message);
                }

                ViewBag.Name = "Projects/" + projectName + "/Activities/" + activityName;
                return PartialView("_OneDriveFilesForActivity", model);
            }

            throw new InvalidOperationException("Catch this: ");
        }

        #endregion
    }
}