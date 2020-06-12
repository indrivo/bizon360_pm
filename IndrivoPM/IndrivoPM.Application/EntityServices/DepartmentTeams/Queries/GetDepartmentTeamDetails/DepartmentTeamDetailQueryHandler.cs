using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamDetails
{
    public class DepartmentTeamDetailQueryHandler : IRequestHandler<DepartmentTeamDetailQuery, DepartmentTeamDetailModel>
    {
        private readonly IGearContext _context;

        public DepartmentTeamDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<DepartmentTeamDetailModel> Handle(DepartmentTeamDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.DepartmentTeams
                .Include(x => x.Department)
                .Include(x => x.DepartmentTeamLead)
                .Include(x => x.JobDepartmentTeams).ThenInclude(x => x.JobPosition)
                .Include(x => x.UserDepartmentTeams).ThenInclude(x => x.User)
                .FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(DepartmentTeam), request.Id);
            }

            return DepartmentTeamDetailModel.Create(entity);
        }
    }
}
