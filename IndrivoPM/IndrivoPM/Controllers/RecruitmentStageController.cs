using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.AddRecruitmentStage;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.DeleteRecruitmentStage;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineList;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.RenameRecruitmentStage;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.UpdateRecruitmentStage;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageDetails;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    public class RecruitmentStageController : BaseController
    {
        #region SelectList Methods

        private async Task<List<SelectListItem>> GetRecruitingPipelineSelectList()
        {
            var request = await Mediator.Send(new GetRecruitingPipelineListQuery());
            return request?.PipelineLookupModels.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        #endregion

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllRecruitmentStages()
        {
            var recruitmentStages = await Mediator.Send(new GetRecruitmentStageListQuery());

            ViewBag.Searching = false;

            return PartialView("_RecruitmentStagesList", recruitmentStages);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.RecruitingPipelineSelectList = await GetRecruitingPipelineSelectList();

            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateRecruitmentStageCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetRecruitmentStageDetailsQuery { Id = id.Value });

            if (request == null) return BadRequest();

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Rename(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetRecruitmentStageDetailsQuery { Id = id.Value });

            if (request == null) return BadRequest();

            return PartialView("_Rename", Mapper.Map<RenameRecruitmentStageCommand>(request));
        }

        public async Task<IActionResult> Rename(RenameRecruitmentStageCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return RedirectToAction("Details", new { id = command.Id });
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetRecruitmentStageDetailsQuery { Id = id.Value });

            if (request == null) return BadRequest();

            ViewBag.RecruitingPipelineSelectList = await GetRecruitingPipelineSelectList();

            return PartialView("_Edit", Mapper.Map<UpdateRecruitmentStageCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateRecruitmentStageCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return RedirectToAction("Details", new { id = command.Id });
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty) return NotFound();

            if (await Mediator.Send(new DeleteRecruitmentStageCommand { Id = id.Value }) == null)
                return BadRequest();

            return new JsonResult("Success");
        }
    }
}