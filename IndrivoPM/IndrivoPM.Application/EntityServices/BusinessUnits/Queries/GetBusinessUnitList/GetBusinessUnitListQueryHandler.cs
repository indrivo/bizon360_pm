using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitList
{
    public class GetBusinessUnitListQueryHandler : IRequestHandler<GetBusinessUnitListQuery, BusinessUnitListViewModel>
    {
        private readonly IGearContext _context;

        public GetBusinessUnitListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<BusinessUnitListViewModel> Handle(GetBusinessUnitListQuery request, CancellationToken cancellationToken)
        {
            return new BusinessUnitListViewModel
            {
                BusinessUnits = await _context.BusinessUnits
                    .OrderBy(x => x.Name)
                    .Include(x => x.Department)
                    .ThenInclude(x => x.DepartmentTeams)
                    .Include(x => x.BusinessUnitLead)
                    .ThenInclude(x => x.JobPosition).Select(x => BusinessUnitLookupModel.Create(x))
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
