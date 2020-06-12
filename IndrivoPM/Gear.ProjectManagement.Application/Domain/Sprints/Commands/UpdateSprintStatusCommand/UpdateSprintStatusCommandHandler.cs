using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.Sprints.Commands.UpdateSprintCommand;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.UpdateSprintStatusCommand
{
    public class UpdateSprintStatusCommandHandler : IRequestHandler<UpdateSprintStatusCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public UpdateSprintStatusCommandHandler(IGearContext context, IUserAccessor userAccessor,
            IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateSprintStatusCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = _context.Sprints.Find(request.Id);
            entity.SprintStatus = request.Status;

            

            _context.Sprints.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var notification = new SprintUpdated()
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                GroupEntityId = entity.ProjectId ?? Guid.NewGuid(),
                GroupEntityName = entity.Project?.Name ?? "",
                Recipients = await _context.ProjectMembers.Include(x => x.User)
                    .Where(x => x.ProjectId == entity.ProjectId).Select(x => x.User.Email).ToListAsync(cancellationToken),
                UserName = user.Identity.Name
            };
            await _mediator.Publish(notification, cancellationToken);


            return Unit.Value;
        }
    }
}
