using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Background.PlanedTasks
{
    public class PlanedTasksScopedService : IScopeBase<PlanedTasksScopedService>
    {
        private readonly IGearContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;
        private readonly IMediator _mediator;

        public PlanedTasksScopedService(IGearContext context, IMediator mediator, 
            UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task ExecuteProcess()
        {
            if (_environment.IsProduction())
            {
                var yesterday = DateTime.Today.AddDays(-1);

                var projects = _context.Projects.Include(x => x.Activities);
                var owners = (await _userManager.GetUsersInRoleAsync("PM Owner")).Where(x => x.Active)
                    .Select(x => x.UserName).ToList();

                foreach (var project in projects)
                {
                    var activities = project.Activities
                        .Where(x => x.StartDate >= yesterday && x.StartDate < DateTime.Today
                            && !(x.ActivityStatus == ActivityStatus.Completed 
                                 || x.ActivityStatus == ActivityStatus.Refused
                                 || x.ActivityStatus == ActivityStatus.Tested)).ToList();

                    if (activities.Any())
                    {
                        // Send notification.
                        await _mediator.Publish(new PlanedTasksNotification
                        {
                            Activities = activities,
                            Recipients = owners
                        }, CancellationToken.None);
                    }
                }
            }
        }
    }
}
