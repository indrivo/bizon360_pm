using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizon360.Models;
using Bizon360.Utils;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.ActivateDepartmentTeam;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignDepartmentTeamLeader;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignJobPosition;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignUser;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.CreateDepartmentTeam;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.DeleteDepartmentTeam;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.MoveDepartmentTeam;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.RemoveJobPosition;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.RenameDepartmentTeam;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.UpdateDepartmentTeam;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.UpdateOrderDepartmentTeam;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamDetails;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsByParent;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    public class DepartmentTeamsController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Adm;
        }

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        [HasPermission(Permissions.DepartmentTeamRead)]
        public async Task<IActionResult> Index()
        {
            return View(await Mediator.Send(new GetDepartmentTeamListQuery()));
        }

        [HasPermission(Permissions.DepartmentTeamRead)]
        public async Task<IActionResult> GetTeamList(Guid? parentId)
        {
            if (parentId == null || parentId == Guid.Empty)
                return PartialView("_TeamList", await Mediator.Send(new GetDepartmentTeamListQuery()));

            var teamsByParent = await Mediator.Send(new GetDepartmentTeamsByParentQuery
            {
                DepartmentId = parentId.Value
            });

            return PartialView("AdmViews/_TeamsFormDepartment", teamsByParent);
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamCreate)]
        public async Task<ActionResult> Create(Guid? departmentId)
        {
            var department = await Mediator.Send(new GetDepartmentListQuery());
            ViewBag.Departments = department.Departments.Count == 0 ? null : department.Departments;

            var jobs = await Mediator.Send(new GetJobPositionListQuery());
            ViewBag.JobPositions = jobs.JobPositions.Count == 0 ? null : jobs.JobPositions;

            if (departmentId == null || departmentId == Guid.Empty) return PartialView("_Create");

            return PartialView("_CreateInDepartment", new CreateDepartmentTeamCommand { DepartmentId = departmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.DepartmentTeamCreate)]
        public async Task<ActionResult> Create( CreateDepartmentTeamCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();
            var request = await Mediator.Send(new DepartmentTeamDetailQuery { Id = id.Value });

            if (request == null)
                return BadRequest();

            var department = Mediator.Send(new GetDepartmentListQuery()).Result
                .Departments.Where(x => x.IsDeletable == true).ToList();

            var jobPositions = Mediator.Send(new GetJobPositionListQuery()).Result.JobPositions.ToList();

            ViewBag.Departments = department.Count == 0 ? null : department;

            ViewBag.JobPositions = jobPositions.Count == 0 ? null : jobPositions; 

            return PartialView("_Edit", Mapper.Map<UpdateDepartmentTeamCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> Edit(UpdateDepartmentTeamCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Json(new { success = true, message = "Success!" });
        }

        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> DeactivateTeam(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            var  command = new ActivateDepartmentTeamCommand { Ids = id, Active = false};

            await Mediator.Send(command);

            return Json(new { id = parentId });
        }

        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> ActivateTeam(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            var  command = new ActivateDepartmentTeamCommand { Ids = id, Active = true};

            await Mediator.Send(command);

            return Json(new {id = parentId});
        }

        [HasPermission(Permissions.DepartmentTeamDelete)]
        public async Task<ActionResult> Delete(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            if (await Mediator.Send(new DeleteDepartmentTeamCommand { Ids = id }) == null)
                return BadRequest();

            return Json(new {id = parentId});
        }


        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.DepartmentTeamRead)]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new DepartmentTeamDetailQuery() { Id = id.Value });

            if (request == null) return BadRequest();

            return View(request);
        }

        #region Quick actions

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> UpdateOrder(List<Guid> itemIds)
        {
            var command = new UpdateOrderDepartmentTeamCommand { DepartmentTeamIds = itemIds };

            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> AssignUser(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var team = await Mediator.Send(new DepartmentTeamDetailQuery { Id = id });

            if (team == null) return NotFound();

            var request = new AssignUserCommand
            {
                DepartmentTeamId = team.Id,
                DepartmentId = team.DepartmentId,
                ApplicationUserIds = team.MembersIds
            };

            return PartialView("_AssignUser", request);
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> AssignUser(AssignUserCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> AssignJobPosition(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var jobPositions = Mediator.Send(new GetJobPositionListQuery()).Result.JobPositions.ToList();
            ViewBag.JobPositions = jobPositions.Count == 0 ? null : jobPositions;

            var team = await Mediator.Send(new DepartmentTeamDetailQuery { Id = id });

            if (team == null) return NotFound();

            return PartialView("_AssignJobPosition", new AssignJobPositionCommand { Id = team.Id, JobPositionIds = team.JobPositionIds});
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> AssignJobPosition(AssignJobPositionCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> RenameTeam(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var team = await Mediator.Send(new DepartmentTeamDetailQuery {Id = id});

            if (team == null) return NotFound();

            var request = new RenameDepartmentTeamCommand
            {
                Id = team.Id,
                Name = team.Name,
                DepartmentId = team.DepartmentId
            };

            return PartialView("_RenameTeam", request);
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> RenameTeam(RenameDepartmentTeamCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> AssignLeader(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var team = await Mediator.Send(new DepartmentTeamDetailQuery {Id = id});

            return PartialView("_AssignLeader", new AssignDepartmentTeamLeaderCommand { Id = id, DepartmentId = team.DepartmentId });
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> AssignLeader(AssignDepartmentTeamLeaderCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> RemoveJobFromTeam(Guid parentId, List<Guid> id)
        {
            var command = await Mediator.Send(new RemoveJobPositionCommand
            {
                DepartmentTeamId = parentId,
                JobPositionIds = id
            });

            if (command != Unit.Value) return BadRequest();

            if (parentId == Guid.Empty) return Ok();

            var request = await Mediator.Send(new GetJobPositionListQuery() { DepartmentTeamId = parentId });

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

        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> MoveTeam(List<Guid> id)
        {
            var departments = await Mediator.Send(new GetDepartmentListQuery());
            var teams = await Mediator.Send(new GetDepartmentTeamListQuery());

            ViewBag.Departments = departments.Departments.ToList();
            ViewBag.DepartmentTeams = teams.DepartmentTeams.ToList();



            return PartialView("_MoveTeam", new MoveDepartmentTeamCommand { DepartmentTeamIds = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> MoveTeam(MoveDepartmentTeamCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }
        #endregion

    }
}