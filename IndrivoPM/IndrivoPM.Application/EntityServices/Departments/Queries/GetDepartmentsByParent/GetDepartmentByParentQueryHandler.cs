using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentsByParent
{
   public class GetDepartmentByParentQueryHandler : IRequestHandler<GetDepartmentByParentQuery, DepartmentByParentModel>
    {
        private readonly IGearContext _context;

        public GetDepartmentByParentQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<DepartmentByParentModel> Handle(GetDepartmentByParentQuery request, CancellationToken cancellationToken)
        {
            var departments = await _context.Departments
                .OrderBy(x => x.Name)
                .Where(x => x.IsDeletable && x.BusinessUnitId == request.BusinessUnitId)
                .Include(x => x.BusinessUnit)
                .Include(x => x.DepartmentLead)
                .Include(x => x.DepartmentTeams)
                .Select(x => DepartmentLookupByParentModel.Create(x)).ToListAsync(cancellationToken);

            return new DepartmentByParentModel()
            {
                Departments = departments,
                BusinessUnitId = request.BusinessUnitId,
            };
        }
    }
}
