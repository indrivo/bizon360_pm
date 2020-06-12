using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.MoveToProjectGroup
{
    public class MoveToProjectGroupCommandHandler : IRequestHandler<MoveToProjectGroupCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public MoveToProjectGroupCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(MoveToProjectGroupCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.Projects.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Project), request.Id);
            }

            entity.ProjectGroupId = request.ProjectGroupId;
            

            _context.Projects.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
