using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bizon360.Models;
using Bizon360.Utils;
using Bizon360.ViewComponents;
using Bizon360.Views.Projects;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.Common.Extensions.Result;
using Gear.Domain.PmEntities.Enums;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Identity.Permissions.Infrastructure.Utils;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetUsers;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFolderContentIntoAnotherDirectory;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.RefreshUserToken;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.UploadFile;
using Gear.Manager.Core.EntityServices.CloudStorage.Querries.GetChildren;
using Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByStatusesReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByTypeReport;
using Gear.Manager.Core.Infrastructure.MediatorResultHandler;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityLists;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.BulkActions.ChangeProjectActivityTypeStates;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetProjectActivityTypes;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupList;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkMoveToProjectGroup;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkSetPriority;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkUpdateStatus;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.DeleteProjects;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.CreateProject;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.DeleteProject;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.InvoiceProject;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.SetUpProject;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.EditProjectMembers;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.MoveToProjectGroup;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.RenameProject;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.SetProjectPriority;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.UpdateProjectStatus;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.UpdateProject;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectDetails;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectInvoice;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectListByGroup;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembers;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectSettings;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsFromGroup;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectTabsNav;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.SearchProjects;
using Gear.Sstp.Abstractions.Domain;
using Gear.Sstp.Abstractions.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    [Authorize]
    public class ProjectsController : BaseController
    {
        private readonly ISstpRepo<SolutionType> _solutionTypeRepo;
        private readonly ISstpRepo<ServiceType> _serviceTypeRepo;
        private readonly ISstpRepo<TechnologyType> _technologyTypeRepo;
        private readonly ISstpRepo<ProductType> _productTypeRepo;
        private readonly IMediatrResultFactory _mediatorFactory;

        public ProjectsController(ISstpRepo<SolutionType> solutionTypeRepo, ISstpRepo<ServiceType> serviceTypeRepo, ISstpRepo<TechnologyType> technologyTypeRepo, ISstpRepo<ProductType> productTypeRepo, IMediatrResultFactory mediatorFactory)
        {
            _solutionTypeRepo = solutionTypeRepo;
            _serviceTypeRepo = serviceTypeRepo;
            _technologyTypeRepo = technologyTypeRepo;
            _productTypeRepo = productTypeRepo;
            _mediatorFactory = mediatorFactory;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Pm;
        }

        #region Projects

        [HasPermission(Permissions.ProjectGroupRead)]
        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public IActionResult Index(ProjectStatus status)
        {
            ViewData["ActivePage"] = ProjectsIndexTabs.GetCurrentPage(status);
            ViewBag.Status = status;
            ViewBag.CurrentView = ProjectsViewHelper.GetUiEnumValue(status);

            return View();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> GetProjectsFromGroup(Guid groupId, ICollection<ProjectStatus> filter, int skip = 0, int size = 10)
        {
            if (groupId.Equals(Guid.Empty)) return UnprocessableEntity();

            var canAccessAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate);

            var request = await Mediator.Send(new GetProjectsFromGroupQuery
            {
                CanAccessAllEntities = canAccessAllEntities,
                GroupId = groupId,
                Filter = filter,
                Skip = skip,
                Size = size
            });

            if (request == null) return BadRequest();

            return PartialView(skip > 0 ? "_ProjectTableRows" : "_Projects", request);
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> SearchProjects(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue)) return BadRequest();

            var canAccessAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate);

            var request = await Mediator.Send(new SearchProjectsQuery
            {
                HasAccessToAllEntities = canAccessAllEntities,
                SearchValue = searchValue
            });

            if (request != null)
            {
                return PartialView("_ProjectSearchResult", request);
            }

            return BadRequest();
        }

        //[HttpGet]
        //[HasPermission(Permissions.ProjectRead)]
        //public async Task<IActionResult> GetCurrentProjects()
        //{
        //    var getAllProjects = User.UserHasThisPermission(Permissions.ProjectUpdate);

        //    var request = await Mediator.Send(new GetProjectGroupsWithProjectsQuery { GetAllProjects = getAllProjects });

        //    if (request != null)
        //    {
        //        ViewBag.Searching = false;
        //        return PartialView("_BizonProjects", request);
        //    }

        //    return BadRequest();
        //}

        //[HttpGet]
        //[HasPermission(Permissions.ProjectRead)]
        //public async Task<IActionResult> GetProjectsByStatus(ICollection<ProjectStatus> filter)
        //{
        //    var getAllProjects = User.UserHasThisPermission(Permissions.ProjectUpdate);
        //    var request = await Mediator.Send(new GetProjectListByStatusQuery { Filter = filter, GetAllProjects = getAllProjects });

        //    if (request != null)
        //    {
        //        return PartialView("_BizonProjectsNoGroups", request);
        //    }

        //    return BadRequest();
        //}

        [HttpGet]
        [HasPermission(Permissions.ProjectCreate)]
        public IActionResult Create(Guid? projectGroupId)
        {
            var projectGroups = Mediator.Send(new GetProjectGroupListQuery{HasAccessToAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate)}).Result;
            if (projectGroups != null)
            {
                ViewBag.ProjectGroups = projectGroups.ProjectGroups.Select(pg => new SelectListItem
                {
                    Value = pg.Id.ToString(),
                    Text = pg.Name
                }).ToList();
            }

            if (!projectGroupId.HasValue || projectGroupId.Value == Guid.Empty)
                return PartialView("_Create");

            return PartialView("_Create", new CreateProjectCommand { Name = string.Empty, ProjectGroupId = projectGroupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectCreate)]
        public async Task<IActionResult> Create(CreateProjectCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                var request = await Mediator.Send(new GetProjectListByGroupQuery { GroupId = command.ProjectGroupId });

                if (request != null)
                {
                    return Json(new
                    {
                        id = request.ProjectGroupId,
                        count = request.ProjectsCount,
                        view = await this.RenderViewAsync("ProjectPartials/_ProjectsTable", request, true)
                    });
                }

                return Ok();
            }

            return BadRequest();
        }

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> Details(Guid id)
        {
            var request = await Mediator.Send(new GetProjectDetailQuery { Id = id });
            if (request != null) return View("BizonDetails", request);

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            var projectGroups = Mediator.Send(new GetProjectGroupListQuery{ HasAccessToAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate) }).Result;
            if (projectGroups != null)
            {
                ViewBag.ProjectGroups = projectGroups.ProjectGroups.Select(pg => new SelectListItem
                {
                    Value = pg.Id.ToString(),
                    Text = pg.Name
                }).ToList();
            }

            ViewBag.ProjectId = id.Value;

            var request = await Mediator.Send(new GetProjectDetailQuery { Id = id.Value });
            if (request != null) return PartialView("_Edit", Mapper.Map<UpdateProjectCommand>(request));

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> Edit(UpdateProjectCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                var request = await Mediator.Send(new GetProjectListByGroupQuery { GroupId = command.ProjectGroupId });

                if (request != null)
                {
                    return Json(new
                    {
                        id = request.ProjectGroupId,
                        count = request.ProjectsCount,
                        view = await this.RenderViewAsync("ProjectPartials/_ProjectsTable", request, true)
                    });
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> EditRedirect(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            var projectGroups = Mediator.Send(new GetProjectGroupListQuery{ HasAccessToAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate) }).Result;
            if (projectGroups != null)
            {
                ViewBag.ProjectGroups = projectGroups.ProjectGroups.Select(pg => new SelectListItem
                {
                    Value = pg.Id.ToString(),
                    Text = pg.Name
                }).ToList();
            }

            ViewBag.ProjectId = id.Value;

            ViewBag.SolutionTypes = _solutionTypeRepo.GetAll().ToList();
            ViewBag.ServiceTypes = _serviceTypeRepo.GetAll().ToList();
            ViewBag.TechnologyTypes = _technologyTypeRepo.GetAll().ToList();
            ViewBag.ProductTypes = _productTypeRepo.GetAll().ToList();

            var request = await Mediator.Send(new GetProjectDetailQuery { Id = id.Value });
            if (request != null) return View("Edit", Mapper.Map<UpdateProjectCommand>(request));

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> EditRedirect(UpdateProjectCommand command)

        {
            if (await Mediator.Send(command) != null)
                return RedirectToAction("Details", new { id = command.Id });

            return View("Edit", command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectDelete)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            if (await Mediator.Send(new DeleteProjectCommand { Id = id.Value }) != null)
                return Json(new { id = id.Value });

            return BadRequest();
        }

        #endregion

        #region Shortcut Actions

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public IActionResult Rename(Guid? id, string name)
        {
            return PartialView("_Rename");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> Rename(RenameProjectCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { id = command.Id, name = command.Name });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public IActionResult EditStatus(Guid? id, ProjectStatus? status)
        {
            return PartialView("_EditStatus");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> EditStatus(UpdateProjectStatusCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { id = command.Id, status = command.Status.ToString() });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public IActionResult SetPriority(Guid id, ActivityPriority priority)
        {
            return PartialView("_SetPriority");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> SetPriority(SetProjectPriorityCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { id = command.Id, priority = command.Priority.ToString() });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public IActionResult MoveToProjectGroup(Guid? id, Guid? projectGroupId)
        {
            var projectGroups = Mediator.Send(new GetProjectGroupListQuery{ HasAccessToAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate) }).Result;
            if (projectGroups != null)
            {
                ViewBag.ProjectGroups = projectGroups.ProjectGroups.Select(pg => new SelectListItem
                {
                    Value = pg.Id.ToString(),
                    Text = pg.Name
                }).ToList();
            }

            ViewBag.OldProjectGroup = projectGroupId;

            return PartialView("_MoveToProjectGroup");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> MoveToProjectGroup(MoveToProjectGroupCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                var request = await Mediator.Send(new GetProjectListByGroupQuery { GroupId = command.ProjectGroupId });

                if (request != null)
                {
                    return Json(new
                    {
                        id = request.ProjectGroupId,
                        count = request.ProjectsCount,
                        view = await this.RenderViewAsync("ProjectPartials/_ProjectsTable", request, true),
                        projectId = command.Id
                    });
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> EditProjectMembers(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetProjectMembersQuery { Id = id.Value });
            if (request != null)
                return PartialView("_EditProjectMembers",
                    new EditProjectMembersCommand
                    {
                        Id = id.Value,
                        Users = request.Users.Select(u => u.Id).ToList()
                    });

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> EditProjectMembers(EditProjectMembersCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                var request = await Mediator.Send(new GetProjectMembersQuery { Id = command.Id });

                if (request != null)
                {
                    ViewBag.ProjectId = command.Id;
                    return Json(new
                    {
                        id = command.Id,
                        view = await this.RenderViewAsync("_ProjectMembers", request, true)
                    });
                }

                return Ok();
            }

            return BadRequest();
        }

        #endregion

        #region Bulk Actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectDelete)]
        public async Task<IActionResult> BulkDelete(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            if (await Mediator.Send(new BulkDeleteProjectCommand { Projects = entities.ToList() }) != null)
                return Json(new { deletedRows = entities });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public IActionResult BulkUpdateStatus(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            return PartialView("BulkPartials/_BulkUpdateStatus", new BulkUpdateProjectStatusCommand { Projects = entities });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> BulkUpdateStatus(BulkUpdateProjectStatusCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { projects = command.Projects, status = command.Status.ToString() });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public IActionResult BulkSetPriority(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            return PartialView("BulkPartials/_BulkSetPriority", new BulkSetProjectPriorityCommand { Projects = entities });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> BulkSetPriority(BulkSetProjectPriorityCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { projects = command.Projects, priority = command.Priority.ToString() });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public IActionResult BulkMoveToGroup(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            var projectGroups = Mediator.Send(new GetProjectGroupListQuery{ HasAccessToAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate) }).Result;
            if (projectGroups != null)
            {
                ViewBag.ProjectGroups = projectGroups.ProjectGroups.Select(pg => new SelectListItem
                {
                    Value = pg.Id.ToString(),
                    Text = pg.Name
                }).ToList();
            }

            return PartialView("BulkPartials/_BulkMoveToGroup", new BulkMoveToProjectGroupCommand { Projects = entities });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> BulkMoveToGroup(BulkMoveToProjectGroupCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                var request = await Mediator.Send(new GetProjectListByGroupQuery { GroupId = command.ProjectGroupId });

                return Json(new
                {
                    projectGroupId = command.ProjectGroupId,
                    count = request.ProjectsCount,
                    view = await this.RenderViewAsync("ProjectPartials/_ProjectsTable", request, true),
                    projects = command.Projects
                });
            }

            return BadRequest();
        }

        #endregion

        #region Cloud&Api

        [HttpGet]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> GetFiles(string name, Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty)
                return BadRequest();

            Result refreshUserToken = await _mediatorFactory.Execute(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            var dto = new OneDriveTabDto()
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Paths = new List<string>
                {
                    "drive/special/approot:/PM/" + "Projects/" + name + id.Value
                },
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
                    foreach(var path in model.Paths)
                    {
                        model.MetaData.AddRange(await Mediator.Send(new GetChildrenQuery()
                            { ExternalProvider = dto.ExternalProvider, FilePath = path }));
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Catch this: " + e.Message);
                }

                ViewBag.Name = "Projects/" + name;
                return PartialView("_OneDriveTab", model);
            }

            throw new InvalidOperationException("Catch this: ");
        }

        [HttpPost]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> UploadFile(int projectNumber, IFormFile file)
        {
            if (projectNumber < 1)
                return BadRequest();

            await Mediator.Send(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });
            if (file.Length <= 60 * 1024 * 1024 && await Mediator.Send(new UploadFileCommand()
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Filepath = "drive/special/approot:/PM/Projects/" + $"P{projectNumber.ToString().PadLeft(8, '0')}",
                FormFile = file
            }) != null) return Ok();
            return BadRequest();
        }

        #endregion

        #region Tabs

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.LogTimeRead)]
        public async Task<IActionResult> LoggedTime(Guid id)
        {
            var isUserPm = User.UserHasThisPermission(Permissions.ProjectUpdate);
            var request = await Mediator.Send(new GetProjectTabsNavCommand { Id = id, IsUserPm = isUserPm });
            if (request != null) return View(request);

            return BadRequest();
        }

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> Files(Guid id)
        {
            var request = await Mediator.Send(new GetProjectTabsNavCommand { Id = id });
            if (request != null) return View(request);

            return BadRequest();
        }

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> GitLab(Guid id)
        {
            var request = await Mediator.Send(new GetProjectTabsNavCommand { Id = id });
            if (request != null) return View(request);

            return BadRequest();
        }

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> Settings(Guid id)
        {
            var request = await Mediator.Send(new GetProjectTabsNavCommand { Id = id });
            if (request != null) return View(request);

            return BadRequest();
        }

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> Wiki(Guid id)
        {
            var isUserPm = User.UserHasThisPermission(Permissions.ProjectUpdate);
            var request = await Mediator.Send(new GetProjectTabsNavCommand { Id = id, IsUserPm = isUserPm });
            if (request != null) return View(request);

            return BadRequest();
        }

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> Reports(List<Guid> activityListIds, Guid id)
        {
            var isUserPm = User.UserHasThisPermission(Permissions.ProjectUpdate);
            var request = await Mediator.Send(new GetProjectTabsNavCommand { Id = id, IsUserPm = isUserPm });

            if (request == null) return BadRequest();

            var activityList = await Mediator.Send(new GetActivityListsQuery { ProjectId = id });

            ViewBag.ActivityList = activityList.ActivityLists;

            return View(request);
        }


        //[HttpGet]
        //[Breadcrumb("ViewData.Title", FromAction = "Details")]
        //[HasPermission(Permissions.ProjectUpdate)]
        //public async Task<IActionResult> EditWiki(Guid? id)
        //{
        //    if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

        //    var request = await Mediator.Send(new GetProjectDetailQuery { Id = id.Value });
        //    if (request != null)
        //    {
        //        // Used to append project id to the parent breadcrumb node
        //        ViewBag.LinkToReplace = Url.Action("Details", "Projects");
        //        ViewBag.BreadcrumbLink = Url.Action("Details", "Projects", new { id });
        //        return View(Mapper.Map<UpdateWikiCommand>(request));
        //    }

        //    return BadRequest();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[HasPermission(Permissions.ProjectUpdate)]
        //public async Task<IActionResult> EditWiki(UpdateWikiCommand command)
        //{
        //    if (await Mediator.Send(command) != null)
        //        return RedirectToAction("Wiki", new { id = command.Id });

        //    return View(command);
        //}

        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> SetUp(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetProjectSettingsQuery() { ProjectId = id.Value });
            if (request != null) return PartialView("_SetUp", Mapper.Map<SetUpProjectCommand>(request));

            return BadRequest();
        }


        [HttpPost]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> SetUp(SetUpProjectCommand command)
        {
            if (await Mediator.Send(command) != null)
                return RedirectToAction("Settings", new { id = command.ProjectId });

            return BadRequest();
        }

        /// <summary>
        /// Gets project invoice and maps to command.
        /// </summary>
        /// <param name="projectId">
        /// Pass project id to get invoice.
        /// </param>
        [HttpGet]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> Invoice(Guid projectId)
        {
            if (projectId == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetProjectInvoiceQuery() { ProjectId = projectId });
            if (request == null) return BadRequest();

            return PartialView("_Invoice", Mapper.Map<InvoiceProjectCommand>(request));
        }


        [HttpPost]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> Invoice(InvoiceProjectCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Ok();

            return BadRequest();
        }

        #endregion

        #region Dashboard

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> Dashboard(Guid id)
        {
            var model = await Mediator.Send(new GetProjectDetailQuery {Id = id});
            if (model != null) return View(model);

            return BadRequest();
        }
        #endregion
        
        #region Activity Types
        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectRead)]
        public async Task<IActionResult> ActivityTypes(Guid projectId)
        {
            var model = await Mediator.Send(new GetProjectDetailQuery { Id = projectId });
            var listResult = await Mediator.Send(new GetProjectActivityTypesQuery { ProjectId = projectId });
            model.ProjectActivityTypes = listResult.ProjectActivityTypes;
            if (model != null) return View(model);

            return BadRequest();
        }

        [HttpPost]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ProjectUpdate)]
        public async Task<IActionResult> ActivityTypes(ProjectDetailModel model)
        {
            var activeElements = model.ProjectActivityTypes.Where(x => x.Active).Select(x => x.ActivityTypeId).ToList();
            if (activeElements.Any())
            {
                if (await Mediator.Send(new ChangeProjectActivityTypeStatesCommand
                {
                    ProjectId = model.Id,
                    ActivityTypeIds = activeElements,
                    Active = true
                }) == null)
                    return BadRequest();
            }

            var inactiveElements = model.ProjectActivityTypes.Where(x => !x.Active).Select(x => x.ActivityTypeId).ToList();
            if (inactiveElements.Any())
            {
                if (await Mediator.Send(new ChangeProjectActivityTypeStatesCommand
                {
                    ProjectId = model.Id,
                    ActivityTypeIds = inactiveElements,
                    Active = false
                }) == null)
                    return BadRequest();
            }

            return RedirectToAction("ActivityTypes", new { projectId = model.Id });
        }
        #endregion

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        public IActionResult StaffAllocation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetStaffAllocation(GetUsersQuery query)
        {
            var request = await Mediator.Send(query);

            if (request != null)
            {
                return Json(new
                {
                    users = request.Users
                });
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> MoveFolderContent()
        {
            var command = new MoveFolderContentIntoAnotherDirectoryCommand
            {
                ExternalProvider = ExternalProviders.OneDrive,
                FolderName = "Tmp",
                NewPath = @"drive/special/approot:/PM/Projects/",
                OldPath = @"drive/special/approot:/PM/Projects/Bizon360 Platform762df025-a069-4427-b764-703b26d11d2c/Activities/Files bug fixing2a46cc75-50b3-494b-b91e-6263d84f8c94",
                ProjectNumber = 0
            };

            var result = await Mediator.Send(command);
            return Content(result != null ? "Ok" : "Fail");
        }
    }
}