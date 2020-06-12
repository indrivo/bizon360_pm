using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.SetProjectPriority
{
    public class SetProjectPriorityCommandHandler : IRequestHandler<SetProjectPriorityCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public SetProjectPriorityCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(SetProjectPriorityCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.Projects.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Project), request.Id);
            }

            entity.Priority = request.Priority;
            
            
            _context.Projects.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
