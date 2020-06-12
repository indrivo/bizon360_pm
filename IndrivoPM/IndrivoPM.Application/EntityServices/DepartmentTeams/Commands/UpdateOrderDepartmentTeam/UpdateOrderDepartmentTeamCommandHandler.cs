using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.UpdateOrderDepartmentTeam
{
    public class UpdateOrderDepartmentTeamCommandHandler : IRequestHandler<UpdateOrderDepartmentTeamCommand>
    {
        private readonly IGearContext _context;

        public UpdateOrderDepartmentTeamCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrderDepartmentTeamCommand request, CancellationToken cancellationToken)
        {
            var count = 1;

            foreach (var item in request.DepartmentTeamIds)
            {
                var entity = await _context.DepartmentTeams.FindAsync(item);

                if (entity == null) throw new NotFoundException(nameof(DepartmentTeam), item);

                entity.RowOrder = count;

                _context.DepartmentTeams.Update(entity);

                count++;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
