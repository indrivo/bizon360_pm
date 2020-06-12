using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Domain.HrmEntities.Enums;
using Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitList;
using Gear.Manager.Core.EntityServices.Candidates.Commands.CreateCandidate;
using Gear.Manager.Core.EntityServices.Candidates.Commands.DeleteCandidate;
using Gear.Manager.Core.EntityServices.Candidates.Commands.QuickAddCandidate;
using Gear.Manager.Core.EntityServices.Candidates.Commands.UpdateCandidate;
using Gear.Manager.Core.EntityServices.Candidates.Commands.UpdateCandidateStatus;
using Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateDetails;
using Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateList;
using Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamDetails;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList;
using Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineList;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageListByPipeline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    public class CandidateController : BaseController
    {
        #region SelectList Methods

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

        private async Task<List<SelectListItem>> GetRecruitmentStageByPipelineSelectList(Guid recruitingPipelineId)
        {
            var request = await Mediator.Send(new GetRecruitmentStageListByPipelineQuery { RecruitingPipelineId = recruitingPipelineId });
            return request?.RecruitmentStages.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
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

        private async Task<List<SelectListItem>> GetRecruitmentStageByStageSelectList(Guid recruitingPipelineId, Guid recruitmentStageId)
        {
            var request = await Mediator.Send(new GetRecruitmentStageListByPipelineQuery { RecruitingPipelineId = recruitingPipelineId });
            return request?.RecruitmentStages.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name,
                Selected = (p.Id == recruitmentStageId)
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetColorTagSelectList()
        {
            return default;
        }

        private async Task<List<SelectListItem>> GetTagSelectList()
        {
            return default;
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

        private async Task<List<SelectListItem>> GetBussinessUnitSelectList()
        {
            var request = await Mediator.Send(new GetBusinessUnitListQuery());
            return request?.BusinessUnits.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetDepartmentByBusinessUnitSelectList(Guid businessUnitId)
        {
            var request = await Mediator.Send(new GetDepartmentListQuery());
            return request?.Departments.Where(x => x.BusinessUnitId == businessUnitId).Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetTeamByDepartmentSelectList(Guid departmentId)
        {
            var request = await Mediator.Send(new GetDepartmentTeamListQuery());
            return request?.DepartmentTeams.Where(x => x.DepartmentId == departmentId).Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name,
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetJobPositionByTeamSelectList(Guid teamId)
        {
            var request = await Mediator.Send(new DepartmentTeamDetailQuery() { Id = teamId });

            return request?.JobPositions.Select(p => new SelectListItem
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

        public async Task<IActionResult> GetAllCandidates(IEnumerable<CandidateStatus> statuses, string searchParam = null)
        {
            ViewBag.Searching = (searchParam?.Length > 0);

            var candidates = await Mediator.Send(new GetCandidateListQuery());

            return PartialView("_CandidatesList", candidates);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.DepartmentSelectList = await GetDepartmentSelectList();
            ViewBag.RecruitmentStageSelectList = await GetRecruitmentStageSelectList();
            ViewBag.RecruitingPipelineSelectList = await GetRecruitingPipelineSelectList();
            ViewBag.ColorTagSelectList = await GetColorTagSelectList();
            ViewBag.TagSelectList = await GetTagSelectList();
            ViewBag.JobPositionSelectList = await GetJobPositionSelectList();

            return View("Create");
        }

        [HttpGet]
        public async Task<IActionResult> CreateCandidateByPipeline(Guid recruitingPipelineId)
        {
            ViewBag.DepartmentSelectList = await GetDepartmentSelectList();
            ViewBag.RecruitmentStageSelectList = await GetRecruitmentStageByPipelineSelectList(recruitingPipelineId);
            ViewBag.RecruitingPipelineSelectList = await GetRecruitingPipelineSelectList();
            ViewBag.ColorTagSelectList = await GetColorTagSelectList();
            ViewBag.TagSelectList = await GetTagSelectList();
            ViewBag.JobPositionSelectList = await GetJobPositionSelectList();

            ViewBag.PipelineId = recruitingPipelineId;

            return View("Create");
        }

        [HttpGet]
        public async Task<IActionResult> CreateCandidateByStage(Guid recruitingPipelineId, Guid recruitmentStageId)
        {
            ViewBag.DepartmentSelectList = await GetDepartmentSelectList();
            ViewBag.RecruitmentStageSelectList = await GetRecruitmentStageByStageSelectList(recruitingPipelineId, recruitmentStageId);
            ViewBag.RecruitingPipelineSelectList = await GetRecruitingPipelineSelectList();
            ViewBag.ColorTagSelectList = await GetColorTagSelectList();
            ViewBag.TagSelectList = await GetTagSelectList();
            ViewBag.JobPositionSelectList = await GetJobPositionSelectList();

            ViewBag.PipelineId = recruitingPipelineId;

            return View("Create");
        }

        [HttpGet]
        public async Task<IActionResult> CreateCandidateQuick(Guid recruitingPipelineId)
        {
            ViewBag.BusinessUnitSelectList = GetBussinessUnitSelectList();
            ViewBag.RecruitmentStageSelectList = await GetRecruitmentStageSelectList();

            ViewBag.RecruitingPipelineId = recruitingPipelineId;

            return PartialView("CandidatePartials/_QuickAdd");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCandidateQuick(QuickAddCandidateCommand command, Guid? recruitingPipelineId)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            if (recruitingPipelineId.HasValue && recruitingPipelineId != Guid.Empty)
            {
                return RedirectToAction("Details", "RecruitingPipeline", new { id = recruitingPipelineId.Value });
            }

            return new JsonResult("Success");
        }


        [HttpPost]
        public async Task<JsonResult> GetDepartmentList(Guid businessUnitId)
        {
            var departments = await GetDepartmentByBusinessUnitSelectList(businessUnitId);

            return Json(departments);
        }

        [HttpPost]
        public async Task<JsonResult> GetTeamList(Guid departmentId)
        {
            var teams = await GetTeamByDepartmentSelectList(departmentId);

            return Json(teams);
        }

        [HttpPost]
        public async Task<JsonResult> GetJobPositionList(Guid teamId)
        {
            var jobPositions = await GetJobPositionByTeamSelectList(teamId);

            return Json(jobPositions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCandidateCommand command, Guid? recruitingPipelineId)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            if (recruitingPipelineId.HasValue && recruitingPipelineId != Guid.Empty)
            {
                return RedirectToAction("Details", "RecruitingPipeline", new { id = recruitingPipelineId.Value });
            }

            return RedirectToAction("Details", "RecruitingPipeline", new { id = recruitingPipelineId });
        }

        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        public async Task<IActionResult> Details(Guid? id, CandidateDetailsOptions options = CandidateDetailsOptions.Candidate)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetCandidateDetailsQuery { Id = id.Value });

            if (request == null) return BadRequest();

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> EditCandidateFromPipeline(Guid? id, Guid recruitingPipelineId)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetCandidateDetailsQuery { Id = id.Value });

            ViewBag.DepartmentSelectList = await GetDepartmentSelectList();
            ViewBag.RecruitmentStageSelectList = await GetRecruitmentStageSelectList();
            ViewBag.RecruitingPipelineSelectList = await GetRecruitingPipelineSelectList();
            ViewBag.ColorTagSelectList = await GetColorTagSelectList();
            ViewBag.TagSelectList = await GetTagSelectList();
            ViewBag.JobPositionSelectList = await GetJobPositionSelectList();

            ViewBag.RecruitingPipelineId = recruitingPipelineId;

            return View("Edit", Mapper.Map<UpdateCandidateCommand>(request));
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid? id, Guid? recruitingPipelineId)
        {
            // TODO: Make candidate not possible to select recruitment stage while the pipeline wasn't selected
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetCandidateDetailsQuery { Id = id.Value });

            ViewBag.DepartmentSelectList = await GetDepartmentSelectList();
            ViewBag.RecruitmentStageSelectList = await GetRecruitmentStageSelectList();
            ViewBag.RecruitingPipelineSelectList = await GetRecruitingPipelineSelectList();
            ViewBag.ColorTagSelectList = await GetColorTagSelectList();
            ViewBag.TagSelectList = await GetTagSelectList();
            ViewBag.JobPositionSelectList = await GetJobPositionSelectList();

            return View("Edit", Mapper.Map<UpdateCandidateCommand>(request));
        }

        [HttpGet]
        public IActionResult EditStatus(Guid? id, CandidateStatus? status)
        {
            return PartialView("CandidatePartials/_EditStatus");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(UpdateCandidateStatusCommand command)
        {
            if (await Mediator.Send(command) != null)
                return Json(new { id = command.Id, status = command.CandidateStatus });

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateCandidateCommand command, Guid? recruitingPipelineId)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            if (recruitingPipelineId.HasValue && recruitingPipelineId != Guid.Empty)
            {
                return RedirectToAction("Details", "RecruitingPipeline", new { id = recruitingPipelineId });
            }

            return RedirectToAction("Details", new { id = command.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty) return NotFound();

            if (await Mediator.Send(new DeleteCandidateCommand { CandidateId = id.Value }) == null)
                return BadRequest();

            return Json(new { id = id.Value });
        }
    }
}