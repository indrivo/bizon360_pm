using System;
using System.Linq;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Background.SprintExpirationCheck
{
    public class SprintExpirationScopedService : IScopeBase<SprintExpirationScopedService>
    {
        private readonly IGearContext _context;

        private readonly IMediator _mediator;

        public SprintExpirationScopedService(IMediator mediator, IGearContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task ExecuteProcess()
        {
            var sprints =_context.Sprints
                .Where(x => x.EndDate == DateTime.Now.Date)
                .Include(x => x.Activities.Where(y => y.Active))
                .Include(x => x.Project)
                .ThenInclude(x => x.ProjectManager)
                .ToList();

            if (!sprints.Any()) return;

            await _mediator.Publish(new SprintExpirationNotification
            {
                Sprints = sprints,
            });
        }
    }
}
