using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizon360.Models;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Manager.Core.EntityServices.BusinessUnits.Commands.ActivateBusinessUnit;
using Gear.Manager.Core.EntityServices.BusinessUnits.Commands.AddDepartmentsToBusinessUnit;
using Gear.Manager.Core.EntityServices.BusinessUnits.Commands.AssignBusinessUnitLeader;
using Gear.Manager.Core.EntityServices.BusinessUnits.Commands.CreateBusinessUnit;
using Gear.Manager.Core.EntityServices.BusinessUnits.Commands.DeleteBusinessUnit;
using Gear.Manager.Core.EntityServices.BusinessUnits.Commands.RemoveDepartment;
using Gear.Manager.Core.EntityServices.BusinessUnits.Commands.RenameBusinessUnit;
using Gear.Manager.Core.EntityServices.BusinessUnits.Commands.UpdateBusinessUnit;
using Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitDetails;
using Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitList;
using Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    public class BusinessUnitsController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Adm;
        }


        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        [HasPermission(Permissions.BusinessUnitRead)]
        public async Task<IActionResult> Index()
        {
            var businessUnits = await Mediator.Send(new GetBusinessUnitListQuery());
            return View(businessUnits);
        }
        
        [HttpGet]
        [HasPermission(Permissions.BusinessUnitCreate)]
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [HasPermission(Permissions.BusinessUnitCreate)]
        public async Task<ActionResult> Create(CreateBusinessUnitCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        [HttpGet]
        [HasPermission(Permissions.BusinessUnitUpdate)]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetBusinessUnitDetailQuery {Id = id});

            return PartialView("_Edit",Mapper.Map<UpdateBusinessUnitCommand>(request));
        }

        [HttpPost]
        [HasPermission(Permissions.BusinessUnitUpdate)]
        public async Task<ActionResult> Edit(UpdateBusinessUnitCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.BusinessUnitRead)]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetBusinessUnitDetailQuery {Id = id});

            if (request == null) Response.StatusCode = 404;

            return View(request);
        }

        [HasPermission(Permissions.BusinessUnitDelete)]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            if (await Mediator.Send(new DeleteBusinessUnitCommand {Id = id}) == null) return BadRequest();

            return new JsonResult("Success");
        }

        #region Aditional Actions

        [HttpGet]
        [HasPermission(Permissions.BusinessUnitRead)]
        public async Task<ActionResult> GetBusinessUnitList()
        {
            var businessUnits = await Mediator.Send(new GetBusinessUnitListQuery());

            if (businessUnits == null) return BadRequest();

            return PartialView("_BusinessUnitList", businessUnits);
        }


        [HttpGet]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public ActionResult AssignLeader(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            return PartialView("_AssignLeader", new AssignBusinessUnitLeaderCommand { Id = id });
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> AssignLeader(AssignBusinessUnitLeaderCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.BusinessUnitUpdate)]
        public async Task<ActionResult> Rename(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var businessUnit = await Mediator.Send(new GetBusinessUnitDetailQuery { Id = id });

            if (businessUnit == null) return NotFound();

            return PartialView("_Rename", new RenameBusinessUnitCommand { Id = businessUnit.Id, Name = businessUnit.Name });
        }

        [HttpPost]
        [HasPermission(Permissions.DepartmentTeamUpdate)]
        public async Task<ActionResult> Rename(RenameBusinessUnitCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }


        [HasPermission(Permissions.BusinessUnitUpdate)]
        public async Task<ActionResult> ActivateBusinessUnit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var command = new ActivateBusinessUnitCommand { Id = id, Active = true};

            await Mediator.Send(command);

            return new JsonResult("Success");
        }

        [HasPermission(Permissions.BusinessUnitUpdate)]
        public async Task<ActionResult> DeactivateBusinessUnit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var command = new ActivateBusinessUnitCommand
            {
                Id = id,
                Active = false
            };

            await Mediator.Send(command);

            return new JsonResult("Success");
        }

        [HttpGet]
        [HasPermission(Permissions.BusinessUnitUpdate)]
        public async Task<ActionResult> AddDepartments(Guid businessUnitId)
        {
            if (businessUnitId == Guid.Empty) return BadRequest();

            var departments = await Mediator.Send(new GetDepartmentListQuery());

            if (departments.Departments.Count == 0)
            {
                ViewBag.Teams = null;
                return PartialView("_AddDepartments");
            }
            var currentDepartmentIds = departments.Departments.Where(x => x.BusinessUnitId == businessUnitId).Select(x => x.Id).ToList();

            ViewBag.Departments = departments.Departments.Where(x => x.IsDeletable).ToList();

            var request = new AddDepartmentsToBusinessUnitCommand
            {
                BusinessUnitId = businessUnitId,
                DepartmentIds = currentDepartmentIds
            };

            return PartialView("_AddDepartments", request);
        }

        [HttpPost]
        [HasPermission(Permissions.BusinessUnitUpdate)]
        public async Task<ActionResult> AddDepartments(AddDepartmentsToBusinessUnitCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return new JsonResult("Success");
        }

        [HasPermission(Permissions.BusinessUnitUpdate)]
        public async Task<ActionResult> RemoveDepartment(Guid? parentId, List<Guid> id)
        {
            if (id == null || id.Count == 0) return NotFound();

            var command = new RemoveDepartmentCommand() {DepartmentIds = id};

            if (await Mediator.Send(command) == null) return BadRequest();

            return Json(new { id = parentId });
        }
        #endregion

    }
}