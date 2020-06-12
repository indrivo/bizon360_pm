using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.UpdateProjectGroup
{
    public class UpdateProjectGroupCommandHandler : IRequestHandler<UpdateProjectGroupCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        private readonly IMediator _mediator;

        public UpdateProjectGroupCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateProjectGroupCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = _context.ProjectGroups.Find(request.Id);
            entity.Name = request.Name;

            
            

            _context.ProjectGroups.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new ProjectGroupUpdated()
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
