using System;
using System.Linq;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDto;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityList;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityProgress;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityTrackers;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.CreateLoggedTime;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.DeleteLoggedTime;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.UpdateLoggedTime;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeDetails;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeList;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeListByProject;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetMonthlyLoggedTimeForEmployee;
using Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetMonthlyLogsByActivity;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;
using Platform = Bizon360.Models.Platform;

namespace Bizon360.Controllers
{
    public class LoggedTimeController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public LoggedTimeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Pm;
        }

        [Breadcrumb("ViewData.Title", FromController = typeof(ProjectsController), FromAction = "Details")]
        [HasPermission(Permissions.LogTimeRead)]
        public async Task<IActionResult> Index(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var request = await Mediator.Send(new GetActivityDtoQuery { Id = id });

            if (request == null) return BadRequest();

            ViewBag.LinkToReplace = Url.Action("Details", "Projects");
            ViewBag.BreadcrumbLink = Url.Action("Index", "Activities", new { id = request.ProjectId });
            return View(request);
        }

        [HasPermission(Permissions.LogTimeRead)]
        public async Task<IActionResult> GetLoggedTime(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetLoggedTimeListQuery { ActivityId = id.Value });

            if (request != null)
                return PartialView("_LoggedTime", request);

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.LogTimeRead)]
        public async Task<IActionResult> GetLoggedTimeByProject(Guid? projectId)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetLoggedTimeListByProjectQuery { ProjectId = projectId.Value });
            if (request != null)
            {
                ViewBag.ProjectId = projectId.Value;

                return PartialView("_LoggedTimeByProject", request);
            }


            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.LogTimeRead)]
        public async Task<IActionResult> GetMonthlyLoggedTimeForEmployee(Guid employeeId, DateTime dateTime, DateTime? endDate)
        {
            var request = await Mediator.Send(new GetMonthlyLoggedTimeForEmployeeQuery
            {
                EmployeeId = employeeId,
                Date = dateTime,
                EndDate = endDate
            });

            if (request != null)
                return Json(new
                {
                    numberOfDays = request.NumberOfDays,
                    daysOfWeek = request.DaysOfWeek,
                    timeLogs = request.TimeLogs,
                    totalPerMonth = request.TotalPerMonth,
                    totalEstimated = request.TotalEstimated
                });

            return BadRequest();
        }

        [HttpGet]
        [HasPermission(Permissions.LogTimeRead)]
        [Breadcrumb("ViewData.SecondNode", FromController = typeof(ApplicationUsersController), FromAction = "Index")]
        public async Task<IActionResult> GetEmployeeLoggedTime(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            ViewBag.FullName = $"{user.FirstName} {user.LastName}";
            return View(id);
        }


        [HttpGet]
        [HasPermission(Permissions.LogTimeRead)]
        public async Task<IActionResult> GetMonthlyLoggedTimeByActivity(Guid employeeId, DateTime startDate, DateTime? endDate)
        {
            var request = await Mediator.Send(new GetMonthlyLogsByActivityQuery
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                EndDate = endDate
            });

            if (request != null)
            {
                return PartialView("_MonthlyLoggedTimeByActivity", request);
            }

            return BadRequest();
        }

        [HasPermission(Permissions.LogTimeCreate)]
        public async Task<IActionResult> Create(Guid? id, DateTime? date, int? progress)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return NotFound();

            var activity = await Mediator.Send(new GetActivityDetailQuery() { Id = id.Value });

            ViewBag.TrackerTypes = Mediator.Send(new GetTrackerTypeListQuery { ActivityTypeId = activity.ActivityTypeId }).Result.Trackers;

            return PartialView("_Create", new CreateLoggedTimeCommand { ActivityId = id.Value, DateOfWork = date ?? DateTime.Now });
        }

        [HasPermission(Permissions.LogTimeUpdate)]
        public async Task<IActionResult> Update(Guid? id, Guid? projectId)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetLoggedTimeDetailQuery
            {
                Id = id.Value
            });

            if (request == null) return BadRequest();

            var activities = await Mediator.Send(new GetActivityListQuery
            {
                ProjectId = projectId.Value,
                UserIsInvolved = true
            });

            var trackerTypes = await Mediator.Send(new GetTrackerTypeListQuery());
            ViewBag.Activities = activities.Activities;
            ViewBag.TrackerTypes = trackerTypes.Trackers;
            ViewBag.ProjectId = projectId.Value;

            var loggedTime = UpdateLoggedTimeCommand.Create(request);

            return PartialView("_Update", loggedTime);
        }

        [HasPermission(Permissions.LogTimeUpdate)]
        public async Task<IActionResult> UpdateActivityLoggedTime(Guid? id, Guid? activityId)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            var request = await Mediator.Send(new GetLoggedTimeDetailQuery { Id = id.Value });

            if (request == null) return BadRequest();

            if (activityId != null)
            {
                var trackerTypes = await Mediator.Send(new GetActivityTrackersQuery { Id = activityId.Value });
                ViewBag.TrackerTypes = trackerTypes.Trackers;
            }


            return PartialView("_UpdateActivityLoggedTime", Mapper.Map<UpdateLoggedTimeCommand>(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetActivityTrackers(Guid id)
        {
            if (id.Equals(Guid.Empty)) return UnprocessableEntity("Id cannot be empty");

            var request = await Mediator.Send(new GetActivityTrackersQuery { Id = id });
            if (request != null)
            {
                return Json(request.Trackers.OrderBy(t => t.RowOrder).Select(t => new
                {
                    value = t.Id.ToString(),
                    text = t.Name
                }).ToList());
            }

            return Json(new { progress = await Mediator.Send(new GetActivityProgressQuery { Id = id }) });
        }

        [HttpPost]
        [HasPermission(Permissions.LogTimeCreate)]
        public async Task<IActionResult> Create(CreateLoggedTimeCommand command)
        {
            var result = await Mediator.Send(command);
            if (result == null)
                return BadRequest();

            return Json(new
            {
                activityId = command.ActivityId,
                time = command.Time,
                progress = result
            });
        }

        [HttpPost]
        [HasPermission(Permissions.LogTimeUpdate)]
        public async Task<IActionResult> Update(UpdateLoggedTimeCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Ok();
        }

        [HasPermission(Permissions.LogTimeCreate)]
        public IActionResult LogTimeFromActivitiesPage(Guid? activityId, int? progress)
        {
            if (!activityId.HasValue || activityId.Value == Guid.Empty) return UnprocessableEntity();

            var trackerTypes = Mediator.Send(new GetActivityTrackersQuery { Id = activityId.Value }).Result;
            if (trackerTypes != null)
            {
                ViewBag.TrackerTypes = trackerTypes.Trackers.OrderBy(t => t.RowOrder);
            }

            return PartialView("_LogTimeFromActivitiesPage", new CreateLoggedTimeCommand { ActivityId = activityId.Value, DateOfWork = DateTime.Now });
        }

        [HttpGet]
        [HasPermission(Permissions.LogTimeCreate)]
        public async Task<IActionResult> LogTimeFromProjectsPage(Guid? projectId)
        {
            if (!projectId.HasValue || projectId.Value == Guid.Empty) return UnprocessableEntity();

            var activities = await Mediator.Send(new GetActivityListQuery
            { ProjectId = projectId.Value, UserIsInvolved = true });
            var trackerTypes = await Mediator.Send(new GetTrackerTypeListQuery());
            ViewBag.Activities = activities.Activities;
            ViewBag.TrackerTypes = trackerTypes.Trackers;
            ViewBag.ProjectId = projectId.Value;

            return PartialView("_LogTimeFromProjectsPage", new CreateLoggedTimeCommand { DateOfWork = DateTime.Now });
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        [HasPermission(Permissions.LogTimeDelete)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            if (await Mediator.Send(new DeleteLoggedTimeCommand { Id = id.Value }) != null)
                return Json(new { id = id.Value });

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetActivityProgress(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty) return UnprocessableEntity();

            return Json(new { progress = await Mediator.Send(new GetActivityProgressQuery { Id = id.Value }) });
        }
    }
}