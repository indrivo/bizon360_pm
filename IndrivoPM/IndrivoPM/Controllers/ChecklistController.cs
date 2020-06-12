using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CompleteCheckItem;
using Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CreateCheckItem;
using Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.DeleteCheckItem;
using Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.RenameCheckItem;
using Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.SwapCheckItems;
using Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Querries.GetCheckListForActivity;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bizon360.Controllers
{
    public class ChecklistController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetChecklistItems(Guid? activityId)
        {
            if (!activityId.HasValue || activityId.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetCheckListForActivityQuery {ActivityId = activityId.Value});
            if (request != null)
                return PartialView("_CheckListItems", request);

            return BadRequest();
        }

        [HttpGet]
        public ActionResult Create(Guid? activityId)
        {
            return PartialView("_Create");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCheckItemCommand command)
        {
            if (await Mediator.Send(command) != null)
                return RedirectToAction("GetChecklistItems", new { activityId = command.ActivityId });

            return BadRequest();
        }

        [HttpGet]
        public ActionResult CompleteChecklistItem(Guid? id, Guid? activityId, Guid activityTypeId, int? progress)
        {
            var request = Mediator.Send(new GetTrackerTypeListQuery{ActivityTypeId = activityTypeId}).Result;
            ViewBag.TrackerTypes = request?.Trackers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            ViewBag.Id = id;
            ViewBag.ActivityId = activityId;

            return PartialView("_CompleteChecklistItem", new CompleteCheckItemCommand
            {
                ActivityId = activityTypeId,
                DateOfWorkValue = DateTime.Now,
                Id = id.Value
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteChecklistItem(CompleteCheckItemCommand command)
        {
            if (await Mediator.Send(command) != null)
                return RedirectToAction("GetChecklistItems", new { activityId = command.ActivityId });

            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> CompleteWithoutLoggingTime(CompleteCheckWithoutLoggingTimeCommand command)
        {
            if (await Mediator.Send(command) != null)
                return RedirectToAction("GetChecklistItems", new { activityId = command.ActivityId });

            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChecklistItem(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            if (await Mediator.Send(new DeleteCheckItemCommand { CheckItemId = id.Value}) != null)
                return Json(new { success = true, message = "Success!" });

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RenameChecklistItem(Guid id, string name)
        {
            if (id == Guid.Empty)
                return NotFound();

            if (await Mediator.Send(new RenameCheckItemCommand { Id = id, Content = name}) != null)
                return Json(new { success = true, message = "Success!" });

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> SwapChecklistItems(IList<Guid> ids)
        {
            if (await Mediator.Send(new SwapCheckItemsCommand { Ids = ids }) != null)
                return Json(new { success = true, message = "Success!" });

            return BadRequest();
        }
    } 
}
