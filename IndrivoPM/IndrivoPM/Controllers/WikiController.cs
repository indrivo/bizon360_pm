using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bizon360.Models;
using Bizon360.Utils;
using Bizon360.ViewComponents;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Identity.Permissions.Infrastructure.Utils;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.DeleteFile;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.RefreshUserToken;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.UploadFile;
using Gear.Manager.Core.EntityServices.CloudStorage.Querries.CheckUserToken;
using Gear.Manager.Core.EntityServices.CloudStorage.Querries.GetChildren;
using Gear.Manager.Core.Infrastructure.MediatorResultHandler;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.CreateHeadline;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.DeleteHeadline;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.UpdateHeadline;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.CreateSection;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.DeleteSection;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.UpdateSection;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectTabsNav;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineDetails;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Sections.GetSectionDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    [Authorize]
    public class WikiController : BaseController
    {
        private readonly IMediatrResultFactory _mediatorFactory;

        public WikiController(IMediatrResultFactory mediatorFactory)
        {
            _mediatorFactory = mediatorFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Pm;
        }
        
        #region Headlines
        [HttpGet]
        [HasPermission(Permissions.WikiRead)]
        [Breadcrumb("ViewData.SecondNode", FromController = typeof(ProjectsController), FromAction = "Index")]
        public async Task<IActionResult> Index(Guid projectId)
        {
            var isUserPm = User.UserHasThisPermission(Permissions.ProjectUpdate);
            var request = await Mediator.Send(new GetProjectTabsNavCommand { Id = projectId, IsUserPm = isUserPm });
            ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());

            if (request == null) return BadRequest();

            return View(request);
        }

        [HttpGet]
        [HasPermission(Permissions.WikiRead)]
        public async Task<IActionResult> GetHeadlines(Guid projectId)
        {
            if (projectId.Equals(Guid.Empty)) return NotFound();

            var request = await Mediator.Send(new GetHeadlineListQuery { ProjectId = projectId });
            ViewBag.IsAuthenticatedOnCloud = await Mediator.Send(new CheckUserTokenQuery());


            if (request == null) return BadRequest();

            return PartialView("_Headlines", request);
        }

        [HttpGet]
        [HasPermission(Permissions.WikiCreate)]
        public IActionResult CreateHeadline(Guid projectId)
        {
            return PartialView("_CreateHeadline", new CreateHeadlineCommand {ProjectId = projectId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.WikiCreate)]
        public async Task<IActionResult> CreateHeadline(CreateHeadlineCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.WikiUpdate)]
        public async Task<IActionResult> EditHeadline(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
                
            var request = await Mediator.Send(new GetHeadlineDetailQuery { Id = id });

            if (request == null) return BadRequest();
            
            return PartialView("_EditHeadline", Mapper.Map<UpdateHeadlineCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.WikiUpdate)]
        public async Task<IActionResult> EditHeadline(UpdateHeadlineCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();
            
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.WikiDelete)]
        public async Task<IActionResult> DeleteHeadline(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            if (await Mediator.Send(new DeleteHeadlineCommand {Id = id}) == null) return BadRequest();

            return Ok();
        }

        #endregion
            
        #region Sections

        [HttpGet]
        [HasPermission(Permissions.WikiCreate)]
        public IActionResult CreateSection(Guid headlineId)
        {
            return PartialView("_CreateSection");
        }

        [HttpPost]
        [HasPermission(Permissions.WikiCreate)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSection(CreateSectionCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            var request = await Mediator.Send(new GetSectionDetailQuery { Id = command.Id });

            if (request != null)
            {
                return Json(new
                {
                    headlineId = command.HeadlineId,
                    view = await this.RenderViewAsync("_Section", request, true)
                });
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> EditSection(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var request = await Mediator.Send(new GetSectionDetailQuery { Id = id });

            if (request == null) return BadRequest(); 
            
            return PartialView("_EditSection", Mapper.Map<UpdateSectionCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSection(UpdateSectionCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSection(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            if (await Mediator.Send(new DeleteSectionCommand { Id = id }) == null) return BadRequest();

            return Ok();
        }

        #endregion

        [HttpGet]
        [HasPermission(Permissions.WikiRead)]
        public async Task<IActionResult> GetFiles(string projectName, Guid projectId, int projectNumber)
        {
            if (projectId == Guid.Empty) return BadRequest();

            var refreshUserToken = await _mediatorFactory.Execute(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            var dto = new OneDriveTabDto()
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Paths = new List<string>
                {
                    "drive/special/approot:/PM/" + "Projects/" + projectName + projectId,
                    "drive/special/approot:/PM/" + "Projects/" + $"P{projectNumber.ToString().PadLeft(8, '0')}"
                },
                FolderName = "   "
            };

            if (!refreshUserToken.IsSuccess) throw new InvalidOperationException("Catch this: ");

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

            ViewBag.Name = "Projects/" + projectName;
            return PartialView("_OneDriveFilesForProject", model);
        }

        [HttpPost]
        [HasPermission(Permissions.WikiUpdate)]
        public async Task<IActionResult> UploadFile(int projectNumber, IFormFile file)
        {
            await Mediator.Send(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });
            if (file.Length <= 60 * 1024 * 1024 && await Mediator.Send(new UploadFileCommand()
            {
                ExternalProvider = ExternalProviders.OneDrive,
                Filepath = "drive/special/approot:/PM/" + "Projects/" + $"P{projectNumber.ToString().PadLeft(8, '0')}",
                FormFile = file
            }) != null)
                return Ok();
            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.WikiDelete)]
        public async Task<IActionResult> DeleteFile(string id, IList<string> paths, string fileName)
        {
            await Mediator.Send(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            try
            {
                var hasOperationCompletedWithSuccess = false;
                foreach(var path in paths)
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
    }
}