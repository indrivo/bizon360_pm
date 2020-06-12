using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.UpdateActivityType
{
    public class UpdateActivityTypeCommandHandler : IRequestHandler<UpdateActivityTypeCommand>
    {
        private readonly IUserAccessor _userAccessor;

        private readonly IGearContext _context;

        private readonly IMediator _mediator;

        public UpdateActivityTypeCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateActivityTypeCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = _context.ActivityTypes.Find(request.Id);

            entity.Abbreviation = request.Abbreviation;
            entity.ColorBadge = request.ColorBadge;
            entity.Name = request.Name;

            

            _context.ActivityTypes.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new ActivityTypeUpdated()
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                UserName = user.Identity.Name
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
