using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bizon360.ViewComponents;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.Common.Extensions.Result;
using Gear.Common.Extensions.StringExtensions;
using Gear.Domain.PmEntities.Enums;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.DeleteFile;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.RefreshUserToken;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.UploadFile;
using Gear.Manager.Core.EntityServices.CloudStorage.Querries.CheckUserToken;
using Gear.Manager.Core.EntityServices.CloudStorage.Querries.GetChildren;
using Gear.Manager.Core.Infrastructure.MediatorResultHandler;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByActivityList;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.CreateActivityList;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.DeleteActivityList;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.RenameActivityList;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityList;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityListStatus;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityListDetails;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityLists;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectDetails;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectName;
using Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using ActivityListViewModel = Bizon360.Models.ActivityListViewModel;

namespace Bizon360.Controllers
{
    [Authorize]
    public class ActivityListsController : BaseController
    {
        private readonly IMediatrResultFactory _mediatorFactory;
        public ActivityListsController(IMediatrResultFactory mediatorFactory)
        {
            _mediatorFactory = mediatorFactory;
        }
        [HttpGet]
        [HasPermission(Permissions.ActivityListRead)]
        public async Task<IActionResult> GetActivityLists(Guid projectId, ICollection<ActivityStatus> filter)
        {
            if (projectId.Equals(Guid.Empty)) return UnprocessableEntity();

            var request = await Mediator.Send(new GetActivityListsQuery { ProjectId = projectId, Filter = filter });

            if (request == null) return BadRequest();

            ViewBag.ProjectId = projectId;

            return PartialView("_ActivityLists", request);
        }

        [HttpGet]
        [Route("{controller}/{id}")]
        [HasPermission(Permissions.ActivityListRead)]
        [Breadcrumb("ViewData.SecondNode", FromController = typeof(ProjectsController), FromAction = "Index")]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await Mediator.Send(new GetActivityListDetailQuery {Id = id});

            var request = await Mediator.Send(new GetActivityListDetailQuery {Id = id});
            if (request == null) return BadRequest();

            ViewBag.LinkToReplace = Url.Action("Details", "Projects");
            ViewBag.BreadcrumbLink = Url.Action("Index", "Activities", new {id = request.ProjectId});
            ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());

            return View(model);
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityListCreate)]
        public async Task<IActionResult> Create(Guid projectId)
        {
            if (projectId == Guid.Empty) return UnprocessableEntity();
            var project = await Mediator.Send(new GetProjectDetailQuery { Id = projectId });
            if (project == null) return BadRequest();

            ViewBag.ProjectNumber = project.Number;
            ViewBag.Sprints = await Mediator.Send(new GetSprintsDtoQuery { ProjectId = projectId });
            ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());

            return PartialView("_Create");
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityListCreate)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateActivityListCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            var activityList = await Mediator.Send(new GetActivityListDetailQuery { Id = command.Id });

            if (command.File != null)
                await UploadFile(command.ProjectNumber, activityList.Number, command.File);

            return PartialView("_ActivityList", new ActivityListViewModel
            {
                Id = command.Id,
                Name = command.Name,
                ProjectId = command.ProjectId
            });
        }
        
        [HttpPost]
        [HasPermission(Permissions.ActivityListCreate)]
        public async Task<IActionResult> UploadFile(int projectNumber, int activityListNumber, IFormFile file)
        {
            var fileSize = 60 * 1024 * 1024;

            if (file.Length > fileSize || projectNumber < 1 || activityListNumber < 1) return BadRequest();

            Result refreshUserToken = await _mediatorFactory.Execute(new RefreshCurrentUserTokenCommand { ExternalProvider = ExternalProviders.OneDrive });

            if (!refreshUserToken.IsSuccess) return BadRequest();

            HttpResponseMessage uploadFileResult = await Mediator.Send(new UploadFileCommand
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Filepath = $"drive/special/approot:/PM/Projects/P{projectNumber.ToString().PadLeft(8, '0')}/ActivityLists/AL{activityListNumber.ToString().PadLeft(8, '0')}",
                FormFile = file
            });

            if (uploadFileResult.IsSuccessStatusCode)
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityListUpdate)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var request = await Mediator.Send(new GetActivityListDetailQuery { Id = id });

            if (request == null) return BadRequest();

            ViewBag.CurrentStatus = request.ActivityListStatus;
            ViewBag.Sprints = await Mediator.Send(new GetSprintsDtoQuery { ProjectId = request.ProjectId });
            ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());

            var project = await Mediator.Send(new GetProjectDetailQuery { Id = request.ProjectId });
            var model = UpdateActivityListCommand.Create(request);
            model.ProjectNumber = project.Number;
            
            return PartialView("_Edit", model);
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityListUpdate)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateActivityListCommand command)
        {
            var projectName = await Mediator.Send(new GetProjectNameQuery { Id = command.ProjectId });

            if (await Mediator.Send(command) == null) return BadRequest();

            if (command.File != null)
                await UploadFile(command.ProjectNumber, command.ActivityListNumber, command.File);

            return Json(new
            {
                id = command.Id,
                status = command.ActivityListStatus,
                name = command.Name
            });

        }

        [HttpGet]
        [HasPermission(Permissions.ActivityListUpdate)]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var request = await Mediator.Send(new GetActivityListDetailQuery { Id = id });

            if (request != null)
            {
                ViewBag.CurrentStatus = request.ActivityListStatus;

                return PartialView("_UpdateStatus", Mapper.Map<UpdateActivityListStatusCommand>(request));
            }

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityListUpdate)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateActivityListStatusCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                return Json(new
                {
                    id = command.Id,
                    status = command.ActivityListStatus
                });
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityListUpdate)]
        public async Task<IActionResult> Rename(Guid id)
        {
            var request = await Mediator.Send(new GetActivityListDetailQuery { Id = id });

            if (request != null)
            {
                return PartialView("_Rename", Mapper.Map<RenameActivityListCommand>(request));
            }

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityListUpdate)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rename(RenameActivityListCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                return Json(new
                {
                    id = command.Id,
                    name = command.Name
                });
            }

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityListDelete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id.Equals(Guid.Empty)) return NotFound();

            if (await Mediator.Send(new DeleteActivityListCommand { Id = id }) != null)
            {
                return Json(new
                {
                    id = id
                });
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> SearchActivitiesByActivityList(Guid projectId, string searchValue)
        {
            if (projectId.Equals(Guid.Empty) || string.IsNullOrEmpty(searchValue)) return UnprocessableEntity();

            var request = await Mediator.Send(new SearchActivitiesByActivityListQuery
            { ProjectId = projectId, SearchValue = searchValue });

            if (request != null)
            {
                ViewBag.ProjectId = projectId;

                return PartialView("_ActivityListSearchResult", request);
            }

            return BadRequest();
        }

        #region Cloud & API
        [HttpPost]
        [HasPermission(Permissions.ChangeRequestUpdate)]
        public async Task<IActionResult> DeleteFile(string id, string path, string alternativePath, string fileName)
        {
            await Mediator.Send(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            var oldVersionRequest = new DeleteFileCommand
            {
                Id = id,
                ExternalProvider = ExternalProviders.OneDrive,
                FilePath = path,
                FileName = fileName
            };

            var request = new DeleteFileCommand
            {
                Id = id,
                ExternalProvider = ExternalProviders.OneDrive,
                FilePath = alternativePath,
                FileName = fileName
            };

            try
            {
                if (await Mediator.Send(oldVersionRequest) != null || await Mediator.Send(request) != null)
                    return Ok(request.Id);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Catch this: " + e.Message);
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityListRead)]
        public async Task<IActionResult> GetFiles(string projectName, Guid? projectId, string activityListName, Guid entityId, int projectNumber, int activityListNumber)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return BadRequest();

            Result refreshUserToken = await _mediatorFactory.Execute(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            var dto = new OneDriveTabDto()
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Paths = new List<string>
                {
                    "drive/special/approot:/PM/" + "Projects/" + projectName + projectId.Value + "/"
                    + "ActivityLists/" + (activityListName?.ReplaceSpecialCharacters('_') ?? string.Empty) + entityId,
                    "drive/special/approot:/PM/" + "Projects/" + $"P{projectNumber.ToString().PadLeft(8, '0')}/"
                    + "ActivityLists/" + $"AL{activityListNumber.ToString().PadLeft(8, '0')}"},
                FolderName = "   "
            };

            if (refreshUserToken.IsSuccess)
            {
                var model = new OneDriveListViewModel
                {
                    Paths = dto.Paths,
                    FolderName = dto.FolderName,
                    MetaData = new List<CloudMetaData>()
                };

                try
                {
                    foreach (var path in model.Paths)
                    {
                        model.MetaData.AddRange(await Mediator.Send(new GetChildrenQuery()
                            { ExternalProvider = dto.ExternalProvider, FilePath = path }));
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Catch this: " + e.Message);
                }

                ViewBag.Name = "Projects/" + projectName + projectId.Value + "/ActivityLists/" +
                               (activityListName?.ReplaceSpecialCharacters('_') ?? string.Empty) + entityId;
                return PartialView("_OneDriveFilesForActivityList", model);
            }

            throw new InvalidOperationException("Catch this: ");
        }
        #endregion
    }
}