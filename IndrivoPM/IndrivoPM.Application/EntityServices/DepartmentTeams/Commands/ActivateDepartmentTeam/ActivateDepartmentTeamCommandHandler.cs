using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.ActivateDepartmentTeam
{
    public class ActivateDepartmentTeamCommandHandler : IRequestHandler<ActivateDepartmentTeamCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public ActivateDepartmentTeamCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(ActivateDepartmentTeamCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            if (request.Ids.Count != 0)
            {
                foreach (var item in request.Ids)
                {
                    var entity = _context.DepartmentTeams.Find(item);

                    if (request.Active)
                        entity.Activate();
                    else
                        entity.Deactivate();

                    _context.DepartmentTeams.Update(entity);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
