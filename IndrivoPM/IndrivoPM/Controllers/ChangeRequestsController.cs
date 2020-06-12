using System;
using System.Collections.Generic;
using System.Linq;
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
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.BulkActions.BulkDeleteRequest;
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.BulkActions.BulkRefuseRequest;
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.CreateChangeRequest;
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.DeleteChangeRequest;
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.RefuseChangeRequest;
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.UpdateChangeRequest;
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestDetails;
using Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestList;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectDetails;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectList;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectTabsNav;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    public class ChangeRequestsController : BaseController
    {
        private readonly IMediatrResultFactory _mediatorFactory;

        public ChangeRequestsController(IMediatrResultFactory mediatorFactory)
        {
            _mediatorFactory = mediatorFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Pm;
        }

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromController = typeof(ProjectsController), FromAction = "Index")]
        [HasPermission(Permissions.ChangeRequestRead)]
        public async Task<IActionResult> Index(Guid id)
        {
            ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());

            var isUserPm = User.UserHasThisPermission(Permissions.ProjectUpdate);
            var request = await Mediator.Send(new GetProjectTabsNavCommand { Id = id, IsUserPm = isUserPm });
            if (request != null) return View(request);

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ChangeRequestRead)]
        public async Task<IActionResult> GetChangeRequests(Guid projectId)
        {
            var request = await Mediator.Send(new GetChangeRequestListQuery
            {
                ProjectId = projectId
            });

            if (request == null) return BadRequest();

            ViewBag.ProjectId = projectId;
            return PartialView("_ChangeRequests", request);
        }

        [HttpGet]
        [HasPermission(Permissions.ChangeRequestRead)]
        public async Task<IActionResult> GetRequestsWithStatus(Guid projectId, ChangeRequestStatus status)
        {
            var request = await Mediator.Send(new GetChangeRequestListQuery
            {
                ProjectId = projectId,
                Status = status
            });

            if (request == null) return BadRequest();

            ViewBag.ProjectId = projectId;
            ViewBag.Status = status;
            return Json(new
            {
                view = await this.RenderViewAsync("_RequestsByStatus", request, true),
                count = request.ChangeRequests.Count
            });
        }

        [HttpGet]
        [HasPermission(Permissions.ChangeRequestCreate)]
        public async Task<IActionResult> Create(Guid projectId)
        {
            var request = await Mediator.Send(new GetProjectDetailQuery {Id = projectId});
            if (request == null)
                return BadRequest();

            ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());

            ViewBag.EntityId = Guid.NewGuid();

            ViewBag.ProjectId = projectId;

            ViewBag.ProjectName = request.Name;

            ViewBag.Projects = GetProjectSelectList();

            ViewBag.ProjectNumber = request.Number;

            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ChangeRequestCreate)]
        public async Task<IActionResult> Create(CreateChangeRequestCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                var entity = await Mediator.Send(new GetChangeRequestDetailsQuery { Id = command.EntityId });
                if (command.File != null)
                    await UploadFile(command.ProjectNumber, entity.Number, command.File);

                return Ok();
            }

            return BadRequest();
        }
        [HttpPost]
        [HasPermission(Permissions.ActivityUpdate)]
        public async Task<IActionResult> UploadFile(int projectNumber, int changeRequestNumber, IFormFile file)
        {
            await Mediator.Send(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });
            if (file.Length <= 60 * 1024 * 1024 && await Mediator.Send(new UploadFileCommand()
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Filepath = "drive/special/approot:/PM/" + "Projects/" + $"P{projectNumber.ToString().PadLeft(8, '0')}" + "/"
                           + "Requests/" + $"R{changeRequestNumber.ToString().PadLeft(8, '0')}",
                FormFile = file
            }) != null) return Ok();
            return BadRequest();
        }
        [HttpGet]
        [HasPermission(Permissions.ChangeRequestRead)]
        public async Task<IActionResult> Details(Guid id)
        {
            ChangeRequestDetailModel request = await Mediator.Send(new GetChangeRequestDetailsQuery { Id = id });
            if (request != null)
            {
                ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());
                return PartialView("_Details", request);
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ChangeRequestUpdate)]
        public async Task<IActionResult> Edit(Guid id)
        {
            ChangeRequestDetailModel request = await Mediator.Send(new GetChangeRequestDetailsQuery { Id = id });
            if (request != null)
            {
                ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());
                ViewBag.Projects = GetProjectSelectList();

                var updateCommand = new UpdateChangeRequestCommand
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    ProjectId = request.ProjectId,
                    ProjectName = request.ProjectName,
                    Priority = request.Priority,
                    Status = request.Status,
                    ChangeRequestNumber = request.Number,
                    ProjectNumber = request.ProjectNumber
                };

                return PartialView("_Edit", updateCommand);
            }

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ChangeRequestUpdate)]
        public async Task<IActionResult> Edit(UpdateChangeRequestCommand command)
        {
            if (command.File != null)
                await UploadFile(command.ProjectNumber, command.ChangeRequestNumber, command.File);

            if (await Mediator.Send(command) != null)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ChangeRequestDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await Mediator.Send(new DeleteChangeRequestCommand {Id = id}) != null)
                return Json(new { id = id });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ChangeRequestReject)]
        public IActionResult RefuseChangeRequest(Guid id)
        {
            ViewBag.requestId = id;
            return PartialView("_RefuseRequestComment");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ChangeRequestReject)]
        public async Task<IActionResult> RefuseChangeRequest(RefuseChangeRequestCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Ok(command);

            return BadRequest();
        }




        #region Bulk Actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ChangeRequestDelete)]
        public async Task<IActionResult> BulkDelete(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            if (await Mediator.Send(new BulkDeleteRequestCommand {Requests = entities.ToList()}) != null)
                return Json(new {deletedRows = entities});

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ChangeRequestReject)]
        public async Task<IActionResult> BulkRefuse(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            if (await Mediator.Send(new BulkRefuseRequestCommand {Requests = entities.ToList()}) != null)
                return Json(new { deletedRows = entities });

            return BadRequest();
        }

        #endregion

        #region SelectList Methods

        private List<SelectListItem> GetProjectSelectList()
        {
            var getAllProjects = User.UserHasThisPermission(Permissions.ProjectUpdate);
            var request = Mediator.Send(new GetProjectListQuery{ GetAllProjects = getAllProjects }).Result;
            return request?.Projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        #endregion

        #region Cloud & Api

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.ChangeRequestRead)]
        public async Task<IActionResult> Files(Guid id)
        {
            var request = await Mediator.Send(new GetChangeRequestDetailsQuery { Id = id });
            if (request != null) return View(request);

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.ChangeRequestUpdate)]
        public async Task<IActionResult> DeleteFile(string id, IList<string> paths, string fileName)
        {
            await Mediator.Send(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            try
            {
                var hasCompletedOperationWithSuccess = false;
                foreach (var path in paths)
                {
                    var request = new DeleteFileCommand
                    {
                        Id = id,
                        ExternalProvider = ExternalProviders.OneDrive,
                        FilePath = path,
                        FileName = fileName
                    };
                    hasCompletedOperationWithSuccess |= (await Mediator.Send(request) != null);
                }
                if (hasCompletedOperationWithSuccess)
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
        public async Task<IActionResult> GetFiles(string projectName, Guid? projectId, string changeRequestName, Guid entityId, 
            int projectNumber, int changeRequestNumber)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return BadRequest();

            Result refreshUserToken = await _mediatorFactory.Execute(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            var dto = new OneDriveTabDto()
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Paths = new List<string>
                {
                    "drive/special/approot:/PM/" + "Projects/" + projectName + projectId.Value + "/"
                    + "Requests/" + (changeRequestName?.ReplaceSpecialCharacters('_') ?? string.Empty) + entityId,
                    "drive/special/approot:/PM/" + "Projects/" + $"P{projectNumber.ToString().PadLeft(8, '0')}" + "/"
                       + "Requests/" + $"R{changeRequestNumber.ToString().PadLeft(8, '0')}"

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

                ViewBag.Name = "Projects/" + projectName + projectId.Value + "/Requests/" +
                               (changeRequestName?.ReplaceSpecialCharacters('_') ?? string.Empty) + entityId;
                return PartialView("_OneDriveFilesForChangeRequest", model);
            }

            throw new InvalidOperationException("Catch this: ");
        }

        #endregion
    }
}