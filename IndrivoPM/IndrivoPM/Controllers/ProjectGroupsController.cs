using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bizon360.Models;
using Bizon360.Utils;
using Gear.Domain.PmEntities.Enums;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Identity.Permissions.Infrastructure.Utils;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.CreateProjectGroup;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.DeleteProjectGroup;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.UpdateProjectGroup;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupDetails;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupList;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectListByGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizon360.Controllers
{
    [Authorize]
    public class ProjectGroupsController : BaseController
    {
        [HttpGet]
        [HasPermission(Permissions.ProjectGroupRead)]
        public async Task<IActionResult> GetProjectGroups(ICollection<ProjectStatus> filter)
        {
            var hasAccessToAllEntities = User.UserHasThisPermission(Permissions.ProjectUpdate);
            var request = await Mediator.Send(new GetProjectGroupListQuery
            {
                HasAccessToAllEntities = hasAccessToAllEntities, 
                Filter = filter
            });

            if (request == null) return BadRequest();

            return PartialView("_ProjectGroups", request);
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectGroupCreate)]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectGroupCreate)]
        public async Task<IActionResult> Create(CreateProjectGroupCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                return PartialView("_ProjectGroup", new ProjectGroupViewModel
                {
                    Id = command.Id,
                    Name = command.Name
                });
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectGroupUpdate)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var request = await Mediator.Send(new GetProjectGroupDetailQuery {Id = id});

            if (request != null)
            {
                return PartialView("_Edit", Mapper.Map<UpdateProjectGroupCommand>(request));
            }

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.ProjectGroupUpdate)]
        public async Task<IActionResult> Edit(UpdateProjectGroupCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                return Json(new
                {
                    id = command.Id,
                    name = command.Name
                });
            }

            return BadRequest();
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            if (await Mediator.Send(new DeleteProjectGroupCommand {Id = id.Value}) == null) return BadRequest();

            var uncategorizedProjects = await Mediator.Send(new GetProjectListByGroupQuery());

            if (uncategorizedProjects != null)
            {
                return Json(new
                {
                    id = id,
                    view = await this.RenderViewAsync("Components/ProjectPartials/_ProjectsTable",
                        uncategorizedProjects, true),
                    count = uncategorizedProjects.ProjectsCount
                });
            }

            return Ok();
        }
    }
}