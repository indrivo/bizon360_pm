using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignDepartmentTeamLeader
{
    public class AssignDepartmentTeamLeaderCommandHandler : IRequestHandler<AssignDepartmentTeamLeaderCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public AssignDepartmentTeamLeaderCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(AssignDepartmentTeamLeaderCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = await _context.DepartmentTeams.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException("Not exist an entity with this id", nameof(DepartmentTeam));
            }

            entity.DepartmentTeamLeadId = request.DepartmentTeamLeadId;
            

            _context.DepartmentTeams.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
