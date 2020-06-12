using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.UpdateOrderDepartment
{
    public class UpdateOrderDepartmentCommandHandler : IRequestHandler<UpdateOrderDepartmentCommand>
    {
        private readonly IGearContext _context;

        public UpdateOrderDepartmentCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrderDepartmentCommand request, CancellationToken cancellationToken)
        {
            var count = 1;

            foreach (var item in request.DepartmentIds)
            {
                var entity = await _context.Departments.FindAsync(item);

                if (entity == null) throw new NotFoundException(nameof(Department), item);

                entity.RowOrder = count;

                _context.Departments.Update(entity);

                count++;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
