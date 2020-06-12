using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.CreateSprintCommand
{
    public class CreateSprintCommandHandler : IRequestHandler<CreateSprintCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        private readonly IMediator _mediator;

        public CreateSprintCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateSprintCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = new Sprint
            {
                ProjectId = request.ProjectId,
                SprintStatus = request.Status,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            entity.CreateEnd(request.Id, request.Name,
                Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            await _context.Sprints.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var users = await _context.Sprints.Include(x => x.Project)
                .ThenInclude(y => y.ProjectMembers)
                .ThenInclude(z => z.User).FirstAsync(x => x.Id == entity.Id, cancellationToken);

            var projectSettings = await _context.Projects.Include(x => x.ProjectSettings)
                .FirstAsync(x => x.Id == request.ProjectId, cancellationToken);

            if (projectSettings.ProjectSettings.SprintNotificationOnCreateUpdateCompleteDelete)
            {
                var notification = new SprintCreated()
                {
                    PrimaryEntityId = entity.Id,
                    PrimaryEntityName = entity.Name,
                    GroupEntityId = entity.ProjectId.Value,
                    GroupEntityName = entity.Project?.Name ?? "",
                    Recipients = users.Project.ProjectMembers.Select(x => x.User.Email).ToList(),
                    UserName = user.Identity.Name
                };

                await _mediator.Publish(notification, cancellationToken);
            }



            return Unit.Value;
        }
    }
}
