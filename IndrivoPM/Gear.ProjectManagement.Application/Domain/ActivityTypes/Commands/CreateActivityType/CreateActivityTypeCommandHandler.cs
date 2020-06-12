using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.CreateActivityType
{
    public class CreateActivityTypeCommandHandler : IRequestHandler<CreateActivityTypeCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;

        private readonly IGearContext _context;

        private readonly IMediator _mediator;

        public CreateActivityTypeCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }
        
        public async Task<Unit> Handle(CreateActivityTypeCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = new ActivityType
            {
                Abbreviation = request.Abbreviation,
                ColorBadge = request.ColorBadge
            };

            var entityId = Guid.NewGuid();
            entity.CreateEnd(entityId, request.Name, userId);

            await _context.ActivityTypes.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var newProjectActivityTypes = await _context.Projects.Select(x => new ProjectActivityType
            {
                ProjectId = x.Id,
                ActivityTypeId = entityId,
                Active = true,
                ModifiedBy = userId,
                ModifyTime = DateTime.Now
            }).ToListAsync(cancellationToken);

            await _context.ProjectActivityTypes.AddRangeAsync(newProjectActivityTypes, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            
            await _mediator.Publish(new ActivityTypeCreated()
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                UserName = user.Identity.Name
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
