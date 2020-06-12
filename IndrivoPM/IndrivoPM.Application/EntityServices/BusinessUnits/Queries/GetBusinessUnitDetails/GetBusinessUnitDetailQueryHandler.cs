using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitDetails
{
    public class GetBusinessUnitDetailQueryHandler : IRequestHandler<GetBusinessUnitDetailQuery, BusinessUnitDetailModel>
    {
        private readonly IGearContext _context;

        public GetBusinessUnitDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<BusinessUnitDetailModel> Handle(GetBusinessUnitDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.BusinessUnits
                .Include(x => x.BusinessUnitLead)
                .ThenInclude(x => x.JobPosition)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(BusinessUnit), request.Id);
            }

            return BusinessUnitDetailModel.Create(entity);
        }
    }
}
