using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.UpdateStatus
{
    public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public UpdateStatusCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.Activities.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            entity.ActivityStatus = request.Status;
            

            _context.Activities.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
