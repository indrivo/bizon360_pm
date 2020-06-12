using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.MoveDepartmentTeam
{
    public class MoveDepartmentTeamCommandHandler : IRequestHandler<MoveDepartmentTeamCommand>
    {
        private readonly IGearContext _context;

        public MoveDepartmentTeamCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(MoveDepartmentTeamCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.DepartmentTeamIds)
            {
                var team = await _context.DepartmentTeams.FindAsync(item);

                team.DepartmentId = request.DepartmentId;

                _context.DepartmentTeams.Update(team);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
