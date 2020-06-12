using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.UpdateSprintCommand
{
    public class UpdateSprintCommandHandler : IRequestHandler<UpdateSprintCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        private readonly IMediator _mediator;

        public UpdateSprintCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateSprintCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.Sprints.FindAsync(request.Id);
            entity.Name = request.Name;
            entity.StartDate = request.StartDate;
            entity.SprintStatus = request.Status;
            entity.EndDate = request.EndDate;

            _context.Sprints.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var dbEntity = await _context.Sprints.Include(x => x.Project)
                .ThenInclude(x => x.ProjectMembers)
                .ThenInclude(x => x.User)
                .FirstAsync(x => x.Id == entity.Id, cancellationToken);

            if (dbEntity.Project.ProjectSettings.SprintNotificationOnCreateUpdateCompleteDelete)
            {
                var notification = new SprintUpdated()
                {
                    PrimaryEntityId = entity.ProjectId ?? Guid.NewGuid(),
                    PrimaryEntityName = entity.Name,
                    Recipients = dbEntity.Project.ProjectMembers.Select(x => x.User.Email).ToList(),
                    GroupEntityId = entity.ProjectId.Value,
                    GroupEntityName = entity.Project?.Name ?? "",
                    UserName = user.Identity.Name
                };

                await _mediator.Publish(notification, cancellationToken);
            }


            return Unit.Value;
        }
    }
}
