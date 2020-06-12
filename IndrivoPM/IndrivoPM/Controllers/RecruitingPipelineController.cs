using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizon360.Models;
using Bizon360.Utils;
using Gear.Domain.HrmEntities.Enums;
using Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkDeleteCandidates;
using Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkMoveCandidateToStage;
using Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkUpdateStatus;
using Gear.Manager.Core.EntityServices.Candidates.Commands.DeleteCandidate;
using Gear.Manager.Core.EntityServices.Candidates.Commands.MoveCandidateToEmployee;
using Gear.Manager.Core.EntityServices.Candidates.Commands.MoveCandidateToStage;
using Gear.Manager.Core.EntityServices.Candidates.Commands.UpdateCandidateStatus;
using Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateDetails;
using Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateList;
using Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateListByStage;
using Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.AddRecruitmentStage;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.CreateRecruitingPipeline;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.DeleteRecruitingPipeline;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.DeleteRecruitmentStage;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.UpdateRecruitingPipeline;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineDetails;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineList;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.MoveStageToPipeline;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.RenameRecruitmentStage;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.UpdateRecruitmentStage;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageDetails;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageListByPipeline;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageListByPipelineAndStatus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    public class RecruitingPipelineController : BaseController
    {
        #region SelectList Methods

        private async Task<List<SelectListItem>> GetCandidateSelectList(Guid stageId)
        {
            var request = await Mediator.Send(new GetCandidateListQuery());

            return request?.CandidateCard
                .Where(x => x.RecruitingStageId != stageId)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Email
                }).OrderBy(x => x.Text).ToList();
        }

        private async Task<List<SelectListItem>> GetRecruitmentStageSelectList(Guid pipelineId)
        {
            var request = await Mediator.Send(new GetRecruitmentStageListByPipelineQuery { RecruitingPipelineId = pipelineId });

            var items = request?.RecruitmentStages
                .Where(x => x.PipelineId != pipelineId)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).OrderBy(x => x.Text).ToList();

            return items;
        }

        private async Task<List<SelectListItem>> GetDepartmentSelectList()
        {
            var request = await Mediator.Send(new GetDepartmentListQuery());
            return request?.Departments.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetRecruitmentStageSelectList()
        {
            var request = await Mediator.Send(new GetRecruitmentStageListQuery());
            return request?.RecruitmentStages.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetRecruitmentStageByPipelineSelectList(Guid? recruitingPipelineId)
        {
            var request = await Mediator.Send(new GetRecruitmentStageListQuery());
            var items = request?.RecruitmentStages.Where(x => x.PipelineId == recruitingPipelineId)
                .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            return items;
        }

        private async Task<List<SelectListItem>> GetRecruitingPipelineSelectList()
        {
            var request = await Mediator.Send(new GetRecruitingPipelineListQuery());
            return request?.PipelineLookupModels.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }


        private async Task<List<SelectListItem>> GetJobPositionSelectList()
        {
            var request = await Mediator.Send(new GetJobPositionListQuery());
            return request?.JobPositions.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        #endregion

        #region Shortcuts

        public async Task<IActionResult> AddStageFromPipeline(Guid pipelineId)
        {
            ViewBag.PipelineId = pipelineId;
            ViewBag.RecruitmentStageSelectList = await GetRecruitmentStageSelectList(pipelineId);

            return PartialView("_AddStageFromPipeline");
        }

        [HttpPost]
        public async Task<IActionResult> AddStageFromPipeline(MoveStageToPipelineCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        public async Task<IActionResult> AddCandidateFromStage(Guid stageId)
        {
            ViewBag.StageId = stageId;
            ViewBag.CandidateSelectList = await GetCandidateSelectList(stageId);

            return PartialView("_AddCandidateFromStage");
        }

        [HttpPost]
        public async Task<IActionResult> AddCandidateFromStage(MoveCandidateToStageCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }
        
        [HttpGet]
        public IActionResult MoveCandidateToEmployee()
        {
            return PartialView("CandidatePartials/_MoveToEmployee");
        }

        [HttpPost]
        public async Task<IActionResult> MoveCandidateToEmployee(MoveCandidateToEmployeeCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        [HttpGet]
        public async Task<IActionResult> MoveCandidateToStage(Guid? id, Guid? recruitmentStageId, Guid? recruitingPipelineId)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var command = new MoveCandidateToStageCommand { CandidateId = id.Value, StageId = recruitmentStageId.Value };

           ViewBag.Stages = await GetRecruitmentStageByPipelineSelectList(recruitingPipelineId.Value);

            return PartialView("CandidatePartials/_MoveToStage", command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveCandidateToStage(MoveCandidateToStageCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Json(new { id = command.CandidateId, stageId = command.StageId });
        }
        #endregion

        #region Bulk Actions

        [HttpGet]
        public IActionResult BulkUpdateStatus(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            return PartialView("BulkPartials/_BulkUpdateStatus", new BulkUpdateCandidateStatusCommand { Candidates = entities });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkUpdateStatus(BulkUpdateCandidateStatusCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { candidates = command.Candidates, status = command.Status.ToString() });

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> BulkMoveToStage(Guid pipelineId, Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            var candidates = await Mediator.Send(new GetCandidateListQuery());
            if (candidates != null)
            {
                ViewBag.Stages = await GetRecruitmentStageSelectList();
            }

            return PartialView("BulkPartials/_BulkMoveToStage", new BulkMoveCandidateToStageCommand { CandidatesId = entities.ToList() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkMoveToStage(BulkMoveCandidateToStageCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                return Json(new
                {
                    stageId = command.StageId,
                    //view = await this.RenderViewAsync("ProjectPartials/_ProjectsTable", request, true),
                    candidates = command.CandidatesId
                });
            }

            return BadRequest();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkDelete(Guid[] entities)
        {
            if (!entities.Any()) return UnprocessableEntity();

            if (await Mediator.Send(new BulkDeleteCandidateCommand { Candidates = entities.ToList() }) != null)
                return Json(new { deletedRows = entities });

            return BadRequest();
        }

        #endregion

        #region Recruitment Stage

        public async Task<IActionResult> GetRecruitmentStageByPipeline(Guid? recruitingPipelineId, CandidateStatus status)
        {
            if (!recruitingPipelineId.HasValue || recruitingPipelineId.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetRecruitmentStageListByPipelineAndStatusQuery
            {
                RecruitingPipelineId = recruitingPipelineId.Value,
                CandidateStatuses = (status == CandidateStatus.New || status == CandidateStatus.Go) 
                ? new List<CandidateStatus>() { CandidateStatus.New, CandidateStatus.Go } 
                : new List<CandidateStatus>() { status }
            });

            if (request == null) return BadRequest();

            var statuses = (status == CandidateStatus.New || status == CandidateStatus.Go) ?
                new List<CandidateStatus>() { CandidateStatus.New, CandidateStatus.Go } :
                new List<CandidateStatus>() { status };

            foreach(var stage in request.RecruitmentStages)
            {
                var actualStageCandidates = new List<CandidateLookupModel>();
                foreach(var candidate in stage.Candidates)
                {
                    if (statuses.Contains(candidate.CandidateStatus) && candidate.Active)
                    {
                        actualStageCandidates.Add(candidate);
                    }
                }
                stage.Candidates = actualStageCandidates;
            }

            ViewBag.Searching = false;

            switch(status)
            {
                case CandidateStatus.NoGo:
                    return PartialView("RecruitingPipelinePartials/_NoGo", request);
                case CandidateStatus.Lost:
                    return PartialView("RecruitingPipelinePartials/_Lost", request);
                case CandidateStatus.Won:
                    return PartialView("RecruitingPipelinePartials/_Won", request);
            }

            return PartialView("RecruitmentStagePartials/_List", request);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchValue)
        {
            var request = await Mediator.Send(new GetRecruitmentStageListQuery());
            if (request != null)
            {
                ViewBag.HasResults = false;
                foreach (var stage in request.RecruitmentStages)
                {
                    stage.Candidates = stage.Candidates
                        .Where(p => p.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                    if (!ViewBag.HasResults && stage.Candidates.Any())
                        ViewBag.HasResults = true;
                }

                ViewBag.Searching = true;
                return PartialView("RecruitmentStagePartials/_List", request);
            }

            return null;
        }

        [HttpGet]
        public IActionResult CreateRecruitmentStage()
        {
            return PartialView("RecruitmentStagePartials/_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecruitmentStage(CreateRecruitmentStageCommand command)
        {
            if (await Mediator.Send(command) != null)
                return PartialView("RecruitmentStagePartials/_Create",
                    new RecruitmentStageViewModel
                    {
                        Id = command.RecruitingPipelineId,
                        Name = command.Name
                    });

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecruitmentStage(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            if (await Mediator.Send(new UpdateRecruitmentStageCommand { Id = id.Value }) != null)
            {
                var request = await Mediator.Send(new GetRecruitmentStageListQuery());
                if (request != null)
                {
                    return Json(new { id = id, view = await this.RenderViewAsync("RecruitmentStagePartials/_RecruitmentStagesTable", request, true) });
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecruitmentStage(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            if (await Mediator.Send(new DeleteRecruitmentStageCommand { Id = id.Value }) != null)
            {
                var request = await Mediator.Send(new GetRecruitmentStageListQuery());
                if (request != null)
                {
                    return Json(new { id = id, view = await this.RenderViewAsync("RecruitmentStagePartials/_RecruitmentStagesTable", request, true) });
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> RenameRecruitmentStage(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetRecruitmentStageDetailsQuery { Id = id.Value });

            if (request == null) return BadRequest();

            return PartialView("RecruitmentStagePartials/_Rename", Mapper.Map<RenameRecruitmentStageCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenameRecruitmentStage(RenameRecruitmentStageCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Json(new { id = command.Id, name = command.Name });
        }

        #endregion

        #region Candidate

        [HttpGet]
        public async Task<IActionResult> GetCandidatesByStatus(IEnumerable<CandidateStatus> statuses, Guid stageId)
        {
            var request = await Mediator.Send(new GetCandidateListByStageQuery { Statuses = statuses, RecruitmentStageId = stageId });

            if (request != null)
            {
                ViewBag.Searching = false;
                return PartialView("RecruitmentStagePartials/_List", request);
            }

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCandidate(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            if (await Mediator.Send(new DeleteCandidateCommand { CandidateId = id.Value }) != null)
                return Json(new { id = id.Value });

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCandidateStatus(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetCandidateDetailsQuery { Id = id.Value });

            if (request == null) return BadRequest();

            return PartialView("CandidatePartials/_UpdateStatus", Mapper.Map<UpdateCandidateStatusCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCandidateStatus(UpdateCandidateStatusCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Json(new { id = command.Id, name = command.CandidateStatus });
        }

        #endregion

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllRecruitingPipelines()
        {
            var recruitingPipeline = await Mediator.Send(new GetRecruitingPipelineListQuery());

            ViewBag.Searching = false;

            return PartialView("_RecruitingPipelineList", recruitingPipeline);
        }

        public IActionResult Create()
        {
            return PartialView("RecruitingPipelinePartials/_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateRecruitingPipelineCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        public async Task<IActionResult> Details(Guid? id, CandidateStage status = CandidateStage.Current)
        {
            ViewBag.Status = status;
            ViewBag.Id = id;

            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetRecruitingPipelineDetailsQuery { Id = id.Value });

            if (request == null) return BadRequest();

            ViewBag.CurrentView = status;

            return View(request);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetRecruitingPipelineDetailsQuery { Id = id.Value });

            return PartialView("RecruitingPipelinePartials/_Edit", Mapper.Map<UpdateRecruitingPipelineCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateRecruitingPipelineCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return RedirectToAction("Details", new { id = command.Id });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty) return NotFound();

            if (await Mediator.Send(new DeleteRecruitingPipelineCommand { Id = id.Value }) == null)
                return BadRequest();

            return new JsonResult("Success");
        }
    }
}