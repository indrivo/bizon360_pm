using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityList;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityListStatus
{
    public class UpdateActivityListStatusCommandHandler : IRequestHandler<UpdateActivityListStatusCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public UpdateActivityListStatusCommandHandler(IGearContext context, IUserAccessor userAccessor,
            IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateActivityListStatusCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = _context.ActivityLists.Find(request.Id);
            entity.ActivityListStatus = request.ActivityListStatus;

            

            _context.ActivityLists.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var notification = new ActivityListUpdated()
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                GroupEntityId = entity.ProjectId,
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
