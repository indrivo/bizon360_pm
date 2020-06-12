using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.CreateProjectGroup
{
    public class CreateProjectGroupCommandHandler : IRequestHandler<CreateProjectGroupCommand, Unit>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        private readonly IMediator _mediator;

        public CreateProjectGroupCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateProjectGroupCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = new ProjectGroup();
            entity.CreateEnd(request.Id, request.Name, Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            await _context.ProjectGroups.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var notification = new ProjectGroupCreated()
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                UserName = user.Identity.Name
            };

            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
