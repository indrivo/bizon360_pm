using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizon360.Models;
using Bizon360.Utils;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitList;
using Gear.Manager.Core.EntityServices.Departments.Commands.ActivateDepartment;
using Gear.Manager.Core.EntityServices.Departments.Commands.AddTeamToDepartment;
using Gear.Manager.Core.EntityServices.Departments.Commands.AssignDepartmentLeader;
using Gear.Manager.Core.EntityServices.Departments.Commands.CreateDepartment;
using Gear.Manager.Core.EntityServices.Departments.Commands.DeleteDepartament;
using Gear.Manager.Core.EntityServices.Departments.Commands.MoveDepartment;
using Gear.Manager.Core.EntityServices.Departments.Commands.RemoveDepartmentTeam;
using Gear.Manager.Core.EntityServices.Departments.Commands.RenameDepartment;
using Gear.Manager.Core.EntityServices.Departments.Commands.UpdateDepartment;
using Gear.Manager.Core.EntityServices.Departments.Commands.UpdateOrderDepartment;
using Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentDetails;
using Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList;
using Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentsByParent;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    [Authorize]
    public class DepartmentsController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Adm;
        }


        #region CUD

        [HasPermission(Permissions.DepartmentCreate)]
        public async Task<ActionResult> Create(Guid? businessUnitId)
        {
            var businessUnits = await Mediator.Send(new GetBusinessUnitListQuery());
            businessUnits.BusinessUnits = businessUnits.BusinessUnits.ToList();

            ViewBag.BusinessUnits = businessUnits.BusinessUnits.Count == 0 ? null : businessUnits.BusinessUnits;

            if (businessUnitId == null) return PartialView("_Create");

            var department = new CreateDepartmentCommand {BusinessUnitId = businessUnitId.Value};
            return PartialView("_CreateDepartmentInBU", department);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.DepartmentCreate)]
        public async Task<ActionResult> Create(CreateDepartmentCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            var request = await Mediator.Send(new GetDepartmentListQuery() { BusinessUnitId = command.BusinessUnitId });
            
            if (request != null)
            {
                return Json(new
                {
                    id = request.BusinessUnitId,
                    count = request.Departments.Count,
                    view = await this.RenderViewAsync("_DepartmentsTable", request, true)
                });
            }

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetDepartmentDetailQuery { Id = id });

            var businessUnits = await Mediator.Send(new GetBusinessUnitListQuery());

            businessUnits.BusinessUnits = businessUnits.BusinessUnits.Where(x => x.IsDeletable).ToList();

            ViewBag.BusinessUnits = businessUnits.BusinessUnits.Count == 0 ? null : businessUnits.BusinessUnits;

            return PartialView("_Edit", Mapper.Map<UpdateDepartmentCommand>(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> Edit(UpdateDepartmentCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();
            
            return Ok();
        }

        [HasPermission(Permissions.DepartmentDelete)]
        public async Task<ActionResult> Delete(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            if (await Mediator.Send(new DeleteDepartmentCommand { Ids = id }) == null)
                return BadRequest();

            return Json(new { id = parentId });
        }

        #endregion

        #region Read

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        [HasPermission(Permissions.DepartmentRead)]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetDepartmentList(Guid? parentId)
        {
            if (parentId == null || parentId == Guid.Empty)
            {
                var departments = await Mediator.Send(new GetDepartmentListQuery());

                if (departments == null) return BadRequest();

                return PartialView("_DepartmentList", departments);
            }

            var departmentsByParent = await Mediator.Send(new GetDepartmentByParentQuery
                {BusinessUnitId = parentId.Value});

            return PartialView("AdmViews/_DepartmentsFromBU", departmentsByParent);
        }

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.DepartmentRead)]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetDepartmentDetailQuery { Id = id});

            if (request == null) return BadRequest();

            ViewBag.DepartmentId = id;

            return View(request);
        }

        #endregion

        #region Quick Actions

        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> AssignLeader(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var department = await Mediator.Send(new GetDepartmentDetailQuery() { Id = id });
            if (department == null) return NotFound();

            return PartialView("_AssignLeader", new AssignDepartmentLeaderCommand { Id = id, BusinessUnitId = department.BusinessUnitId});
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> AssignLeader(AssignDepartmentLeaderCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> Rename(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var department = await Mediator.Send(new GetDepartmentDetailQuery() {Id = id});

            if (department == null) return NotFound();

            var request = new RenameDepartmentCommand
            {
                Id = department.Id,
                Name = department.Name,
                BusinessUnitId = department.BusinessUnitId
            };

            return PartialView("_Rename", request);
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> Rename(RenameDepartmentCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> MoveDepartment(List<Guid> id)
        {
            var businessUnits = await Mediator.Send(new GetBusinessUnitListQuery());
            ViewBag.BusinessUnits = businessUnits.BusinessUnits;

            if (id.Count != 1) return PartialView("_MoveDepartment", new MoveDepartmentCommand {DepartmentIds = id});

            var department = await Mediator.Send(new GetDepartmentDetailQuery() {Id = id.First()});
            if (department == null) return BadRequest();

            var command = new MoveDepartmentCommand
            {
                DepartmentIds = new List<Guid> { department.Id },
                BusinessUnitId = department.BusinessUnitId
            };

            return PartialView("_MoveDepartment", command);
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> MoveDepartment(MoveDepartmentCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return new JsonResult("Success");
        }

        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> DeactivateDepartment(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            var command = new ActivateDepartmentCommand { Ids = id, Active = false};

            await Mediator.Send(command);

            return Json(new { id = parentId });
        }

        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> ActivateDepartment(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            var command = new ActivateDepartmentCommand { Ids = id, Active = true};

            await Mediator.Send(command);

            return Json(new { id = parentId });
        }

        [HttpGet]
        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> AddTeamsToDepartment(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var teams = await Mediator.Send(new GetDepartmentTeamListQuery());

            if (teams.DepartmentTeams.Count == 0)
            {
                ViewBag.Teams = null;
                return PartialView("_AddTeams");
            }

            var currentTeamDepartmentIds = teams.DepartmentTeams.Where(x => x.DepartmentId == id).Select(x => x.Id).ToList();
            var department = await Mediator.Send(new GetDepartmentDetailQuery {Id = id});

            ViewBag.Teams = teams.DepartmentTeams.ToList();

            var request = new AddTeamToDepartmentCommand
            {
                DepartmentId = id,
                DepartmentTeamIds = currentTeamDepartmentIds,
                BusinessUnitId = department.BusinessUnitId
            };

            return PartialView("_AddTeams", request);
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> AddTeamsToDepartment(AddTeamToDepartmentCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return new JsonResult("Success");
        }

        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> RemoveTeamFromDepartment(Guid? parentId, List<Guid> id)
        {
            if (id.Count == 0) return BadRequest();

            var command = new RemoveDepartmentTeamCommand {DepartmentTeamIds = id };

            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Json(new {id = parentId} );
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentUpdate)]
        public async Task<ActionResult> UpdateOrder(List<Guid> itemIds)
        {
            var command = new UpdateOrderDepartmentCommand { DepartmentIds = itemIds };

            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Ok();
        }
        #endregion
    }
}