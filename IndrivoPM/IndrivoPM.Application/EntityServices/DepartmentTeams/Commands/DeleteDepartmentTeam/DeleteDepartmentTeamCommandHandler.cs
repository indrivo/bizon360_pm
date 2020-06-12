using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.DeleteDepartmentTeam
{
    public class DeleteDepartmentTeamCommandHandler : IRequestHandler<DeleteDepartmentTeamCommand>
    {
        private readonly IGearContext _context;

        public DeleteDepartmentTeamCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteDepartmentTeamCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids.Count != 0)
            {
                foreach (var item in request.Ids)
                {
                    var entity = await _context.DepartmentTeams.FindAsync(item);
                    if (entity == null) throw new NotFoundException(nameof(DepartmentTeams), item);
                    _context.DepartmentTeams.Remove(entity);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
