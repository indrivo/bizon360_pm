using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bizon360.Models;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.ActivateActivityType;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.CreateActivityType;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.DeleteActivityType;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.RenameActivityType;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.UpdateActivityType;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.UpdateOrderActivityType;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeDetails;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeList;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.ActivateTrackerType;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.CreateTrackerType;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.DeleteTrackerType;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.MoveTrackerType;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.UpdateOrderTrackerType;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.UpdateTrackerType;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeDetails;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    [Authorize]
    public class ServicesController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Adm;
        }

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        [HasPermission(Permissions.ActivityTypeRead)]
        public IActionResult Index()
        {
            return View();
        }

        #region SubType(Trackers)

        [HttpGet]
        [HasPermission(Permissions.ActivityTrackerRead)]
        public async Task<IActionResult> GetTrackersByActivity(Guid parentId)
        {
            var trackerTypes = await Mediator.Send(new GetTrackerTypeListQuery { ActivityTypeId = parentId } );

            return PartialView("_TrackersList", new TrackerTypeListViewModel { Trackers = trackerTypes.Trackers, ActivityTypeId = parentId });
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityTrackerCreate)]
        public async Task<IActionResult> CreateTracker(Guid activityTypeId)
        {
            var activityTypes = await Mediator.Send(new GetActivityTypeListQuery());

            ViewBag.ActivityTypes = activityTypes.ActivityTypes.Count == 0 ? null : activityTypes.ActivityTypes;

            var request = new CreateTrackerTypeCommand { ActivityTypeId = activityTypeId };

            return PartialView("_CreateTracker", request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityTrackerCreate)]
        public async Task<ActionResult> CreateTracker(CreateTrackerTypeCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityTrackerUpdate)]
        public async Task<ActionResult> EditTracker(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetTrackerTypeDetailQuery { Id = id.Value });

            if (request == null) return BadRequest();

            var activityTypes = await Mediator.Send(new GetActivityTypeListQuery());

            ViewBag.ActivityTypes = activityTypes.ActivityTypes.Count == 0 ? null : activityTypes.ActivityTypes;

            return PartialView("_EditTracker", Mapper.Map<UpdateTrackerTypeCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityTrackerUpdate)]
        public async Task<ActionResult> EditTracker(UpdateTrackerTypeCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        [HasPermission(Permissions.ActivityTrackerDelete)]
        public async Task<ActionResult> DeleteTracker(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            if (await Mediator.Send(new DeleteTrackerTypeCommand { Ids = id }) == null)
                return BadRequest();

            return Json(new { id = parentId });
        }

        [HasPermission(Permissions.ActivityTrackerUpdate)]
        public async Task<ActionResult> ActivateTrackerType(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            if (await Mediator.Send(new ActivateTrackerTypeCommand { Ids = id, Active = true }) == null)
                return BadRequest();

            return Json(new {id = parentId });
        }

        [HasPermission(Permissions.ActivityTrackerUpdate)]
        public async Task<ActionResult> DeactivateTrackerType(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            if (await Mediator.Send(new ActivateTrackerTypeCommand { Ids = id, Active = false }) == null)
                return BadRequest();

            return Json(new { id = parentId });
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityTrackerUpdate)]
        public async Task<ActionResult> MoveTracker(List<Guid> id)
        {
           var activityTypes = await Mediator.Send(new GetActivityTypeListQuery());
            ViewBag.ActivityTypes = activityTypes.ActivityTypes;

            var request = new MoveTrackerTypeCommand { TrackersIds = id };

            return PartialView("_MoveTracker", request);
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityTrackerUpdate)]
        public async Task<ActionResult> MoveTracker(MoveTrackerTypeCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityTrackerUpdate)]
        public async Task<ActionResult> UpdateOrder(List<Guid> itemIds)
        {
            var command = new UpdateOrderTrackerTypeCommand {TrackerTypeIds = itemIds};

            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult(true);
        }


        #endregion

        #region ActivityType

        [HttpGet]
        [HasPermission(Permissions.ActivityTypeRead)]
        public async Task<IActionResult> GetActivityTypesList()
        {
            return PartialView("_ActivityTypesList", await Mediator.Send(new GetActivityTypeListQuery()));
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityTypeCreate)]
        public ActionResult CreateActivityType()
        {
            return PartialView("_CreateActivityType");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityTypeCreate)]
        public async Task<ActionResult> CreateActivityType(CreateActivityTypeCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Ok();

        }

        [HttpGet]
        [HasPermission(Permissions.ActivityTypeUpdate)]
        public async Task<ActionResult> RenameActivityType(Guid id)
        {
            var entity = await Mediator.Send(new GetActivityTypeDetailQuery { Id = id });

            if (entity == null) return BadRequest();

            return PartialView("_RenameActivityType", new RenameActivityTypeCommand { Id = id, Name = entity.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityTypeCreate)]
        public async Task<ActionResult> RenameActivityType(RenameActivityTypeCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Ok();

        }

        [HttpGet]
        [HasPermission(Permissions.ActivityTypeUpdate)]
        public async Task<ActionResult> EditActivityType(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetActivityTypeDetailQuery { Id = id});

            if (request == null) return BadRequest();

            return PartialView("_EditActivityType", Mapper.Map<UpdateActivityTypeCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ActivityTypeUpdate)]
        public async Task<ActionResult> EditActivityType(UpdateActivityTypeCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.ActivityTrackerUpdate)]
        public async Task<ActionResult> ActivityTypeUpdateOrder(List<Guid> itemIds)
        {
            var command = new UpdateOrderActivityTypeCommand { ActivityTypeIds = itemIds };

            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult(true);
        }


        [HasPermission(Permissions.ActivityTypeUpdate)]
        public async Task<ActionResult> ActivateActivityType(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            if (await Mediator.Send(new ActivateActivityTypeCommand { Id = id, Active = true }) == null)
                return BadRequest();

            return Ok();
        }

        [HasPermission(Permissions.ActivityTypeUpdate)]
        public async Task<ActionResult> DeactivateActivityType(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            if (await Mediator.Send(new ActivateActivityTypeCommand { Id = id, Active = false }) == null)
                return BadRequest();

            return Ok();
        }

        [HasPermission(Permissions.ActivityTypeDelete)]
        public async Task<ActionResult> DeleteActivityType(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            if (await Mediator.Send(new DeleteActivityTypeCommand { Id = id }) == null)
                return BadRequest();

            return Ok();
        }

        #endregion
    }
}