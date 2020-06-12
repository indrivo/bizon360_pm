using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizon360.Models;
using Gear.Domain.PmEntities.Enums;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesBySprint;
using Gear.ProjectManagement.Manager.Domain.Sprints.Commands.CreateSprintCommand;
using Gear.ProjectManagement.Manager.Domain.Sprints.Commands.DeleteSprintCommand;
using Gear.ProjectManagement.Manager.Domain.Sprints.Commands.RenameSprintCommand;
using Gear.ProjectManagement.Manager.Domain.Sprints.Commands.UpdateSprintCommand;
using Gear.ProjectManagement.Manager.Domain.Sprints.Commands.UpdateSprintStatusCommand;
using Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintDetailQuery;
using Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintListQuery;
using Microsoft.AspNetCore.Mvc;

namespace Bizon360.Controllers
{
    public class SprintsController : BaseController
    {
        [HttpGet]
        [HasPermission(Permissions.SprintRead)]
        public async Task<IActionResult> GetSprints(Guid projectId, ICollection<ActivityStatus> filter)
        {
            if (projectId.Equals(Guid.Empty)) return UnprocessableEntity();

            var request = await Mediator.Send(new GetSprintListQuery { ProjectId = projectId, Filter = filter });

            if (request == null) return BadRequest();

            ViewBag.ProjectId = projectId;

            return PartialView("_Sprints", request);
        }

        [HttpGet]
        [HasPermission(Permissions.SprintCreate)]
        public IActionResult Create(Guid projectId)
        {
            if (projectId.Equals(Guid.Empty)) return UnprocessableEntity();

            return PartialView("_Create", new CreateSprintCommand
            {
                Name = string.Empty,
                Status = SprintStatus.New,
                ProjectId = projectId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(14)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.SprintCreate)]
        public async Task<IActionResult> Create(CreateSprintCommand command)
        {
            command.Id = Guid.NewGuid();

            if (await Mediator.Send(command) != null)
            {
                ViewBag.ProjectId = command.ProjectId;

                return PartialView("_Sprint", new SprintViewModel
                {
                    Id = command.Id,
                    Name = command.Name,
                    Status = command.Status,
                    DueDate = command.EndDate
                });
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.SprintUpdate)]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id.Equals(Guid.Empty)) return UnprocessableEntity();

            var request = await Mediator.Send(new GetSprintDetailQuery { Id = id });
            if (request != null)
            {
                ViewBag.CurrentStatus = request.Status;
                return PartialView("_Edit", Mapper.Map<UpdateSprintCommand>(request));
            }

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.SprintUpdate)]
        public async Task<IActionResult> Edit(UpdateSprintCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                return Json(new
                {
                    id = command.Id,
                    name = command.Name,
                    status = command.Status,
                    dueDate = $"{command.EndDate:yyyy-MM-dd}"
                });
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.SprintUpdate)]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var request = await Mediator.Send(new GetSprintDetailQuery { Id = id });

            if (request != null)
            {
                ViewBag.CurrentStatus = request.Status;
                return PartialView("_UpdateStatus", Mapper.Map<UpdateSprintStatusCommand>(request));
            }

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.SprintUpdate)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateSprintStatusCommand command)
        {
            if (await Mediator.Send(command) != null)
            {
                return Json(new
                {
                    id = command.Id,
                    status = command.Status
                });
            }

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.SprintUpdate)]
        public async Task<IActionResult> Rename(Guid id)
        {
            var request = await Mediator.Send(new GetSprintDetailQuery { Id = id });

            if (request != null)
            {
                return PartialView("_Rename", Mapper.Map<RenameSprintCommand>(request));
            }

            return BadRequest();
        }

        [HttpPost]
        [HasPermission(Permissions.SprintUpdate)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rename(RenameSprintCommand command)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.SprintDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id.Equals(Guid.Empty)) return UnprocessableEntity();

            if (await Mediator.Send(new DeleteSprintCommand { Id = id }) != null)
                return Json(new { id = id });

            return BadRequest();

        }

        [HttpGet]
        [HasPermission(Permissions.SprintRead)]
        public async Task<IActionResult> GetSprintDates(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetSprintDetailQuery {Id = id.Value});
            if (request != null)
                return Json(new
                {
                    startDate = request.StartDate.ToString("yyyy/MM/dd"),
                    dueDate = request.EndDate.ToString("yyyy/MM/dd")
                });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.SprintRead)]
        public async Task<IActionResult> GetSprintsByProject(Guid? projectId)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetSprintListQuery { ProjectId = projectId.Value });
            if (request != null)
            {
                return Json(request.Sprints.Select(al => new
                {
                    value = al.Id,
                    text = al.Name
                }).ToList());
            }
            
            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.ActivityRead)]
        public async Task<IActionResult> SearchActivitiesBySprint(Guid projectId, string searchValue)
        {
            if (projectId.Equals(Guid.Empty) || string.IsNullOrEmpty(searchValue)) return UnprocessableEntity();

            var request = await Mediator.Send(new SearchActivitiesBySprintQuery
                { ProjectId = projectId, SearchValue = searchValue });

            if (request != null)
            {
                ViewBag.ProjectId = projectId;

                return PartialView("_SprintSearchResult", request);
            }

            return BadRequest();
        }
    }
}