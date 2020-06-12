using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.RemoveDepartmentTeam
{
    public class RemoveDepartmentTeamCommandHandler : IRequestHandler<RemoveDepartmentTeamCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        public RemoveDepartmentTeamCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(RemoveDepartmentTeamCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            var invisibleDepartment = _context.Departments.FirstOrDefault(x => x.IsDeletable == false);
            var teamsToRemove = new List<DepartmentTeam>();

            foreach (var item in request.DepartmentTeamIds)
            {
                var team = _context.DepartmentTeams
                    .First(x => x.Id == item);

                team.DepartmentId = invisibleDepartment.Id;
                team.Deactivate();

                teamsToRemove.Add(team);
            }


            _context.DepartmentTeams.UpdateRange(teamsToRemove);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
