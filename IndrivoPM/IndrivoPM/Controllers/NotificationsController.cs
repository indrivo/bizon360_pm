using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bizon360.Models;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Manager.Core.EntityServices.Departments.Commands.CreateDepartment;
using Gear.Manager.Core.EntityServices.Notifications.Commands.AssignUserNotificationProfile;
using Gear.Manager.Core.EntityServices.Notifications.Commands.ChangeEventSendType;
using Gear.Manager.Core.EntityServices.Notifications.Commands.CreateNotificationProfile;
using Gear.Manager.Core.EntityServices.Notifications.Commands.DeleteNotificationProfile;
using Gear.Manager.Core.EntityServices.Notifications.Commands.Import;
using Gear.Manager.Core.EntityServices.Notifications.Commands.UpdateNotificationProfile;
using Gear.Manager.Core.EntityServices.Notifications.Querries.GetNotificationProfile;
using Gear.Manager.Core.EntityServices.Notifications.Querries.GetNotificationProfileList;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.ProjectManagement.Manager.Domain.Projects.Commands.CreateProject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    public class NotificationsController : BaseController
    {

        private readonly IEventService _eventService;
        private readonly INotificationService _notificationService;

        public NotificationsController(IEventService eventService, INotificationService notificationService)
        {
            _eventService = eventService;
            _notificationService = notificationService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Adm;
        }

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        [HasPermission(Permissions.NotificationProfileRead)]
        public IActionResult Index()
        {
            return View();
        }

        #region Events

        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventService.GetEvents();

            return PartialView("_Events", events);
        }

        public async Task<IActionResult> ChangePropagationType(Guid id, PropagationType type)
        {
            var command = new ChangeEventSendTypeCommand {Id = id, Type = type};

            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }


        [HttpGet]
        [HasPermission(Permissions.NotificationProfileRead)]
        public async Task<IActionResult> ImportEvents()
        {
            var pmAssembly = Assembly.GetAssembly(typeof(ProjectCreated));
            var admAssembly = Assembly.GetAssembly(typeof(DepartmentCreated));

            await Mediator.Send(new ImportCommand(){Assembly = pmAssembly });
            await Mediator.Send(new ImportCommand(){Assembly = admAssembly });

            return Ok();
        }

        #endregion

        #region Notifications UI

        public async Task<IActionResult> GetUserNotifications(string email)
        {
            var notifications = await _notificationService.GetUserNotifications(email);

            return new JsonResult(notifications);
        }

        public async Task<bool> HasNotifications(string email)
        {
            return await _notificationService.HasNotifications(email);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveNotifications(List<Guid> notificationIds)
        {
            if (!notificationIds.Any()) return BadRequest();

            await _notificationService.MarkNotificationsAsRead(notificationIds);
            return Ok();
        }

        #endregion

        #region Profiles

        [HttpGet]
        [HasPermission(Permissions.NotificationProfileRead)]
        public async Task<IActionResult> GetNotificationList()
        {
            var request = await Mediator.Send(new GetNotificationProfileListQuery());

            return PartialView("_NotificationList", request);
        }

        [HttpGet]
        [HasPermission(Permissions.NotificationProfileCreate)]
        public async Task<IActionResult> Create()
        {
            var events = await _eventService.GetEvents();
            ViewBag.AdmEvents = events.Where(x => x.EventGroupName.Contains("Administration")).ToList();
            ViewBag.ProjectEvents = events.Where(x => x.EventGroupName.Contains("Project")).ToList();
            ViewBag.ActivityEvents = events.Where(x => x.EventGroupName.Contains("Activity")).ToList();
            ViewBag.SprintEvents = events.Where(x => x.EventGroupName.Contains("Sprint")).ToList();

            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.NotificationProfileCreate)]
        public async Task<IActionResult> Create(CreateNotificationProfileCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.NotificationProfileUpdate)]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetNotificationProfileDetailQuery() { Id = id });

            var events = await _eventService.GetEvents();
            ViewBag.AdmEvents = events.Where(x => x.EventGroupName.Contains("Administration")).ToList();
            ViewBag.ProjectEvents = events.Where(x => x.EventGroupName.Contains("Project")).ToList();
            ViewBag.ActivityEvents = events.Where(x => x.EventGroupName.Contains("Activity")).ToList();
            ViewBag.SprintEvents = events.Where(x => x.EventGroupName.Contains("Sprint")).ToList();

            if (request == null)
                return BadRequest();

            var vm = new UpdateNotificationProfileCommand()
            {
                Id = request.Id,
                Name = request.Name,
                UserList = request.UserList.Select(x => x.Id).ToList(),
                EventList = request.EventList.Select(x => x.Id).ToList()
            };

            return PartialView("_Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.NotificationProfileUpdate)]
        public async Task<IActionResult> Edit(UpdateNotificationProfileCommand command)
        {
            if (await Mediator.Send(command) == null)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.NotificationProfileUpdate)]
        public async Task<IActionResult> AssignUsers(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var profile = await Mediator.Send(new GetNotificationProfileDetailQuery {Id = id});

            var vm = new AssignUserNotificationProfileCommand
            {
                Id = profile.Id,
                UserListIds = profile.UserList.Select(x => x.Id).ToList()
            };

            return PartialView("_AssignUsers", vm);
        }

        [HttpPost]
        [HasPermission(Permissions.NotificationProfileUpdate)]
        public async Task<IActionResult> AssignUsers(AssignUserNotificationProfileCommand command)
        {
            if (await Mediator.Send(command) == null) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [Breadcrumb("ViewData.SecondNode", FromAction = "Index")]
        [HasPermission(Permissions.NotificationProfileRead)]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var request = await Mediator.Send(new GetNotificationProfileDetailQuery() { Id = id });

            if (request == null) return BadRequest();

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.NotificationProfileDelete)]
        public async Task<IActionResult> Delete(List<Guid> id)
        {
            if (id == null) return NotFound();

            if (await Mediator.Send(new DeleteNotificationProfileCommand() { Ids = id }) == null)
                return BadRequest();

            return Ok();
        }

        #endregion
    }
}