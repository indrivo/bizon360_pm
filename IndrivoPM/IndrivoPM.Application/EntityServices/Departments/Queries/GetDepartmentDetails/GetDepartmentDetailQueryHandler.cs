using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentDetails
{
    public class GetDepartmentDetailQueryHandler : IRequestHandler<GetDepartmentDetailQuery, DepartmentDetailModel>
    {
        private readonly IGearContext _context;

        public GetDepartmentDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<DepartmentDetailModel> Handle(GetDepartmentDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Departments
                .Include(x => x.BusinessUnit)
                .Include(x => x.DepartmentLead)
                    .Include(x => x.DepartmentTeams)
                    .ThenInclude(x => x.DepartmentTeamLead)
                    .Include(x => x.DepartmentTeams)
                    .ThenInclude(x => x.JobDepartmentTeams)
                    .ThenInclude(x => x.JobPosition)
                    .Include(x => x.DepartmentTeams)
                    .ThenInclude(x => x.UserDepartmentTeams)
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Department), request.Id);
            }
            return  DepartmentDetailModel.Create(entity);
        }
    }
}
