using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Identity.Manager.Domain.Roles.Commands.ActivateRole;
using Gear.Identity.Manager.Domain.Roles.Commands.CreateRole;
using Gear.Identity.Manager.Domain.Roles.Commands.DeleteRole;
using Gear.Identity.Manager.Domain.Roles.Commands.UpdateRole;
using Gear.Identity.Manager.Domain.Roles.Queries.GetPermissionList;
using Gear.Identity.Manager.Domain.Roles.Queries.GetRoleDetails;
using Gear.Identity.Manager.Domain.Roles.Queries.GetRoleList;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;
using Platform = Bizon360.Models.Platform;

namespace Bizon360.Controllers
{
    [Authorize]
    public class RolesController : BaseController
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Adm;
        }

        public RolesController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        #region Role Manager

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        [HasPermission(Permissions.RoleRead)]
        public IActionResult Index()
        {
            return View();
        }

        [HasPermission(Permissions.RoleRead)]
        public async Task<IActionResult> GetRoleList()
        {
            var roleList = await Mediator.Send(new GetRoleListQuery());

            return PartialView("_RoleList", roleList);
        }

        [HasPermission(Permissions.RoleRead)]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        public async Task<IActionResult> Details(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return BadRequest();

            var request = await Mediator.Send(new GetRoleDetailQuery() { RoleName = roleName });

            if (request == null) return NotFound();

            return View(request);
        }

        [HasPermission(Permissions.RoleUpdate)]
        public async Task<ActionResult> ActivateRole(List<string> id)
        {
            if (id.Count == 0) return NotFound();

            var command = new ActivateRoleCommand {RoleNames = id, Active = true};

            await Mediator.Send(command);

            return new JsonResult("Success");
        }

        [HasPermission(Permissions.RoleUpdate)]
        public async Task<ActionResult> DeactivateRole(List<string> id)
        {
            if (id.Count == 0) return NotFound();

            var command = new ActivateRoleCommand {RoleNames = id, Active = false};

            await Mediator.Send(command);

            return new JsonResult("Success");
        }

        #region CRUD

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.RoleCreate)]
        public async Task<IActionResult> Create()
        {
            var permissions = await Mediator.Send(new GetPermissionListQuery());

            return View(new CreateRoleCommand() { PermissionsList = permissions });
        }

        [HttpPost]
        [HasPermission(Permissions.RoleCreate)]
        public async Task<ActionResult> Create(CreateRoleCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.RoleUpdate)]
        public async Task<IActionResult> Edit(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return NotFound();

            var entity = await Mediator.Send(new GetRoleDetailQuery() { RoleName = roleName });

            if (entity == null) return BadRequest();

            var permissions = await Mediator.Send(new GetPermissionListQuery());

            var request = new UpdateRoleCommand
            {
                RoleName = entity.Name,
                Description = entity.Description,
                Permissions = entity.Permissions.ToList(),
                PermissionsList = permissions
            };

            return View(request);
        }

        [HttpPost]
        [HasPermission(Permissions.RoleUpdate)]
        public async Task<ActionResult> Edit(UpdateRoleCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return RedirectToAction("Index");
        }

        [HasPermission(Permissions.RoleDelete)]
        public async Task<ActionResult> Delete(List<string> id)
        {
            if (id.Count == 0) return NotFound();

            if (await Mediator.Send(new DeleteRoleCommand { RoleNames = id }) == null)
                return BadRequest();

            return new JsonResult("Success");
        }

        #endregion
        
        #endregion

    }
}