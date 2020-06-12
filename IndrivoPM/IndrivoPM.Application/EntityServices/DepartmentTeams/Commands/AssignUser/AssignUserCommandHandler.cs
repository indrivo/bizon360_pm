using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignUser
{
    public class AssignUserCommandHandler : IRequestHandler<AssignUserCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public AssignUserCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(AssignUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = await _context.DepartmentTeams.FindAsync(request.DepartmentTeamId);

            if (entity == null) throw new NotFoundException("Not exist an entity with this id", nameof(DepartmentTeam));

            // Update many to many UserDepartmentTeams
            var currentUsers = await _context.UserDepartmentTeams.Where(x => x.DeparmentTeamId == request.DepartmentTeamId)
                .Select(x => x.UserId).ToListAsync(cancellationToken);

            // If list of users is null, will remove all users form team.
            if (request.ApplicationUserIds == null || request.ApplicationUserIds.Count == 0)
            {
                foreach (var item in currentUsers)
                {
                    var removeUser = await _context.UserDepartmentTeams.FirstAsync(x =>
                        x.DeparmentTeamId == request.DepartmentTeamId && x.UserId == item, cancellationToken);

                    _context.UserDepartmentTeams.Remove(removeUser);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            // Sort users for adding in team and users for remove from team.
            var addUsers = request.ApplicationUserIds.Except(currentUsers).ToList();
            var removeUsers = currentUsers.Except(request.ApplicationUserIds).ToList();

            if (addUsers.Count != 0)
            {
                foreach (var item in addUsers)
                {
                    var teamUser = new UserDepartmentTeam {DeparmentTeamId = request.DepartmentTeamId, UserId = item};
                     _context.UserDepartmentTeams.Add(teamUser);
                }
            }

            if (removeUsers.Count != 0)
            {
                foreach (var item in removeUsers)
                {
                    var teamUser = _context.UserDepartmentTeams
                        .First(x => x.DeparmentTeamId == request.DepartmentTeamId && x.UserId == item);

                    if (teamUser != null) _context.UserDepartmentTeams.Remove(teamUser);
                }
            }

            

            _context.DepartmentTeams.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
