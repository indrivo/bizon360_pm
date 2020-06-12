using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.AddDepartmentsToBusinessUnit
{
    public class AddDepartmentsToBusinessUnitCommandHandler : IRequestHandler<AddDepartmentsToBusinessUnitCommand>
    {
        private readonly IGearContext _context;

        public AddDepartmentsToBusinessUnitCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddDepartmentsToBusinessUnitCommand request, CancellationToken cancellationToken)
        {
            var checkBusinessUnit = await _context.BusinessUnits
                .AnyAsync(x => x.Id == request.BusinessUnitId, cancellationToken);

            var notDeletableBusinessUnit =await _context.BusinessUnits
                .FirstOrDefaultAsync(x => !x.IsDeletable, cancellationToken);

            var existingDepartments = await _context.Departments.Where(x => x.BusinessUnitId == request.BusinessUnitId)
                .ToListAsync(cancellationToken);

            // Remove all departments form this business unit
            if (request.DepartmentIds == null)
            {
                foreach (var item in existingDepartments)
                {
                    item.BusinessUnitId = notDeletableBusinessUnit.Id;
                    _context.Departments.Update(item);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            if (!checkBusinessUnit) return Unit.Value;

            // Remove departments that has been unselected and add new departments
            var departmentsToAdd = request.DepartmentIds.Except(existingDepartments.Select(x => x.Id)).ToList();
            var departmentsToRemove = existingDepartments.Select(x => x.Id).Except(request.DepartmentIds).ToList();

            if (departmentsToRemove.Count != 0)
            {
                foreach (var item in departmentsToRemove)
                {
                    var departmentRemove = _context.Departments.First(x => x.Id == item);
                    departmentRemove.BusinessUnitId = notDeletableBusinessUnit.Id;
                    _context.Departments.Update(departmentRemove);
                }
            }

            if (departmentsToAdd.Count != 0)
            {
                foreach (var item in departmentsToAdd)
                {
                    var departmentAdd = _context.Departments.First(x => x.Id == item);
                    departmentAdd.BusinessUnitId = request.BusinessUnitId;

                    _context.Departments.Update(departmentAdd);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            

            return Unit.Value;
        }
    }
}
