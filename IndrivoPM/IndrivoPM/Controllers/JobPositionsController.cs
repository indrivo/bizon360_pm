using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bizon360.Models;
using Bizon360.Utils;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList;
using Gear.Manager.Core.EntityServices.JobPositions.Commands.ActivateJobPosition;
using Gear.Manager.Core.EntityServices.JobPositions.Commands.CreateJobPosition;
using Gear.Manager.Core.EntityServices.JobPositions.Commands.DeleteJobPosition;
using Gear.Manager.Core.EntityServices.JobPositions.Commands.RenameJobPosition;
using Gear.Manager.Core.EntityServices.JobPositions.Commands.UpdateJobPosition;
using Gear.Manager.Core.EntityServices.JobPositions.Commands.UpdateOrderJobPosition;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionDetails;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    [Authorize]
    public class JobPositionsController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Adm;
        }

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        [HasPermission(Permissions.JobPositionRead)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HasPermission(Permissions.JobPositionRead)]
        public async Task<IActionResult> GetJobsList(Guid? parentId)
        {
            if (parentId == null || parentId == Guid.Empty)
                return PartialView("_JobsList", await Mediator.Send(new GetJobPositionListQuery()));

            var request = await Mediator.Send(new GetJobPositionListQuery {DepartmentTeamId = parentId.Value});

            if (request == null) return BadRequest();

            return PartialView("AdmViews/_JobsFromTeam", request);
        }


        [HttpGet]
        [HasPermission(Permissions.JobPositionCreate)]
        public async Task<ActionResult> Create(Guid? teamId)
        {
            var teams = await Mediator.Send(new GetDepartmentTeamListQuery());
            ViewBag.DepartmentTeams = teams.DepartmentTeams;

            if (teamId.HasValue && teamId != Guid.Empty)
            {
                return PartialView("_CreateJobInTeam", new CreateJobPositionCommand() {DepartmentTeamId = teamId});
            }

            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.JobPositionCreate)]
        public async Task<ActionResult> Create(CreateJobPositionCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            if (command.DepartmentTeamId != null)
            {
                var request = await Mediator.Send(new GetJobPositionListQuery() { DepartmentTeamId = command.DepartmentTeamId.Value });

                if (request != null)
                {
                    return Json(new
                    {
                        id = request.ParentId,
                        count = request.JobPositions.Count,
                        view = await this.RenderViewAsync("AdmViews/_JobsFromTeam", request, true)
                    });
                }
            }

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.JobPositionUpdate)]
        public async Task<ActionResult> Edit(Guid id, Guid? parentId)
        {
            if (id == Guid.Empty) return NotFound();

            var teams = await Mediator.Send(new GetDepartmentTeamListQuery());
            ViewBag.DepartmentTeams = teams.DepartmentTeams;

            var request = await Mediator.Send(new GetJobPositionDetailQuery { Id = id });

            if (request == null)
                return BadRequest();

            if (parentId.HasValue && parentId != Guid.Empty)
            {
                return PartialView("_EditJobInTeam", Mapper.Map<UpdateJobPositionCommand>(request));

            }

            return PartialView("_Edit", Mapper.Map<UpdateJobPositionCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.JobPositionUpdate)]
        public async Task<ActionResult> Edit(UpdateJobPositionCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            if (command.DepartmentTeamId != null)
            {
                var request = await Mediator.Send(new GetJobPositionListQuery() { DepartmentTeamId = command.DepartmentTeamId.Value });

                if (request != null)
                {
                    return Json(new
                    {
                        id = request.ParentId,
                        count = request.JobPositions.Count,
                        view = await this.RenderViewAsync("AdmViews/_JobsFromTeam", request, true)
                    });
                }
            }

            return Ok();
        }

        [HasPermission(Permissions.JobPositionDelete)]
        public async Task<ActionResult> Delete(List<Guid> id, Guid? parentId)
        {
            if (id.Count == 0) return NotFound();

            if (await Mediator.Send(new DeleteJobPositionCommand { Ids = id }) == null)
                return BadRequest();

            if (parentId != null)
            {
                return Json(new {id = parentId});
            }

            return Ok();
        }


        #region Quick actions

        [HttpGet]
        [HasPermission(Permissions.JobPositionUpdate)]
        public async Task<ActionResult> RenameJobPosition(Guid id, Guid? parentId)
        {
            if (id == Guid.Empty) return BadRequest();

            var job = await Mediator.Send(new GetJobPositionDetailQuery() {Id = id});

            if (job == null)
            {
                return NotFound();
            }

            if (parentId == null || parentId == Guid.Empty)
                return PartialView("_RenameJobPosition", new RenameJobPositionCommand { Id = id, Name = job.Name});


            return PartialView("_RenameJobInTeam", new RenameJobPositionCommand { Id = id, Name = job.Name, ParentId = parentId});
        }

        [HttpPost]
        [HasPermission(Permissions.JobPositionUpdate)]
        public async Task<ActionResult> RenameJobPosition(RenameJobPositionCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        public async Task<ActionResult> DeactivateJobPosition(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return BadRequest();

            var command = new ActivateJobPositionCommand() { Ids = id, Active = false };

            if (await Mediator.Send(command) == null) return BadRequest();

            if (parentId == null || parentId == Guid.Empty) return Ok();

            var request = await Mediator.Send(new GetJobPositionListQuery() { DepartmentTeamId = parentId.Value });

            if (request != null)
            {
                return Json(new
                {
                    id = request.ParentId,
                    count = request.JobPositions.Count,
                    view = await this.RenderViewAsync("AdmViews/_JobsFromTeam", request, true)
                });
            }

            return Ok();
        }

        public async Task<ActionResult> ActivateJobPosition(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return BadRequest();

            var command = new ActivateJobPositionCommand() { Ids = id, Active = true };

            if (await Mediator.Send(command) == null) return BadRequest();

            if (parentId == null || parentId == Guid.Empty) return Ok();

            var request = await Mediator.Send(new GetJobPositionListQuery() { DepartmentTeamId = parentId.Value });

            if (request != null)
            {
                return Json(new
                {
                    id = request.ParentId,
                    count = request.JobPositions.Count,
                    view = await this.RenderViewAsync("AdmViews/_JobsFromTeam", request, true)
                });
            }
            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> UpdateOrder(List<Guid> itemIds)
        {
            var command = new UpdateOrderJobPositionCommand { jobPositionIds = itemIds };

            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Ok();
        }
        #endregion
    }
}