using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Bizon360.Models;
using Gear.Common.Extensions.UserInjection;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserLite;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDto;
using Gear.ProjectManagement.Manager.Domain.MainComments.Commands.CreateMainComment;
using Gear.ProjectManagement.Manager.Domain.MainComments.Commands.DeleteMainComment;
using Gear.ProjectManagement.Manager.Domain.MainComments.Commands.UpdateMainComment;
using Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentDetails;
using Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentList;
using Gear.ProjectManagement.Manager.Domain.SubComments.Commands.CreateSubComment;
using Gear.ProjectManagement.Manager.Domain.SubComments.Commands.DeleteSubComment;
using Gear.ProjectManagement.Manager.Domain.SubComments.Commands.UpdateSubComment;
using Gear.ProjectManagement.Manager.Domain.SubComments.Queries.GetSubCommentDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    public class MainCommentsController : BaseController
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Pm;
        }

        private readonly IUserAccessor _userAccessor;

        public MainCommentsController(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        [Breadcrumb("ViewData.Title", FromController = typeof(ProjectsController), FromAction = "Details")]
        public async Task<IActionResult> Index(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var request = await Mediator.Send(new GetActivityDtoQuery { Id = id });

            if (request == null) return BadRequest();

            ViewBag.LinkToReplace = Url.Action("Details", "Projects");
            ViewBag.BreadcrumbLink = Url.Action("Index", "Activities", new { id = request.ProjectId });

            return View(request);
        }

        #region Main Comments

        [HttpGet]
        public async Task<IActionResult> GetCommentList(Guid recordId)
        {
            if (recordId == Guid.Empty) return BadRequest();

            var userClaims = _userAccessor.GetUser();
            ViewBag.OwnCommentId = Guid.Parse(userClaims.FindFirst(ClaimTypes.NameIdentifier).Value);

            ViewBag.RecordId = recordId;

            var request = await Mediator.Send(new GetMainCommentListQuery() { RecordId = recordId });

            return PartialView("_CommentList", request);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMainComment(CreateMainCommentCommand command)
        {
            if (await Mediator.Send(command) != null) return Ok();

            return BadRequest();
        }


        [HttpGet]
        public async Task<IActionResult> EditMainComment(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var request = await Mediator.Send(new GetMainCommentDetailQuery() {Id = id});

            if (request == null) return BadRequest();

            return PartialView("_EditMainComment",
                new UpdateMainCommentCommand() {Id = request.Id, Message = request.Message});
        }

        [HttpPost]
        public async Task<IActionResult> EditMainComment(UpdateMainCommentCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        public async Task<IActionResult> DeleteMainComment(Guid id)
        {
            await Mediator.Send(new DeleteMainCommentCommand() {Id = id});

            return Ok();
        }

        #endregion

        #region Sub comments

        [HttpGet]
        public async Task<IActionResult> CreateSubComment(Guid id, Guid authorId)
        {
            if (id == Guid.Empty) return BadRequest();

            var user = await Mediator.Send(new GetApplicationUserLiteQuery() {Id = authorId});
            
            return PartialView("_CreateSubComment", new CreateSubCommentCommand() { MainCommentId = id, Message = "@" + user.Username});
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubComment(CreateSubCommentCommand command)
        {
            if (await Mediator.Send(command) != null) return Ok();

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> EditSubComment(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var request = await Mediator.Send(new GetSubCommentDetailQuery() { Id = id });

            if (request == null) return BadRequest();

            return PartialView("_EditSubComment",
                new UpdateSubCommentCommand() { Id = request.Id, Message = request.Message });
        }

        [HttpPost]
        public async Task<IActionResult> EditSubComment(UpdateSubCommentCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }


        public async Task<IActionResult> DeleteSubComment(Guid id)
        {
            await Mediator.Send(new DeleteSubCommentCommand() { Id = id });

            return Ok();
        }

        #endregion

    }
}