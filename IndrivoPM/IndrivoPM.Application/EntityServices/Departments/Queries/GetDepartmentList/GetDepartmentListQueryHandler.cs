using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList
{
   public class GetDepartmentListQueryHandler:IRequestHandler<GetDepartmentListQuery, DepartmentListViewModel>
    {
        private readonly IGearContext _context;

        public GetDepartmentListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<DepartmentListViewModel> Handle(GetDepartmentListQuery request, CancellationToken cancellationToken)
        {
            var departments = await _context.Departments
                .OrderBy(x => x.RowOrder)
                .Include(x => x.BusinessUnit)
                .Include(x => x.DepartmentLead)
                .ThenInclude(x => x.JobPosition)
                .Include(x => x.DepartmentTeams)
                .ThenInclude(x => x.UserDepartmentTeams).ThenInclude(x => x.User)
                .Select(x => DepartmentLookupModel.Create(x)).ToListAsync(cancellationToken);

            if (request.BusinessUnitId == null || request.BusinessUnitId == Guid.Empty)
            {
                return new DepartmentListViewModel
                {
                    Departments = departments
                };
            }

            return new DepartmentListViewModel
            {
                Departments = departments.Where(x => x.BusinessUnitId == request.BusinessUnitId).ToList(),
                BusinessUnitId = request.BusinessUnitId.Value
            };

        }
    }
}
