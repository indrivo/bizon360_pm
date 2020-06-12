using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList
{
    public class GetJobPositionListQueryHandler : IRequestHandler<GetJobPositionListQuery, JobPositionListViewModel>
    {
        private readonly IGearContext _context;

        public GetJobPositionListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<JobPositionListViewModel> Handle(GetJobPositionListQuery request, CancellationToken cancellationToken)
        {
            if (request.DepartmentTeamId == Guid.Empty)
                return new JobPositionListViewModel
                {
                    JobPositions = await _context.JobPositions.OrderBy(x => x.RowOrder)
                        .Select(x => JobPositionLookupModel.Create(x)).ToListAsync(cancellationToken)
                };
            {
                var teamJobs = _context.JobDepartmentTeams.Where(x => x.DepartmentTeamId == request.DepartmentTeamId)
                    .Select(x => x.JobPositionId).ToList();

                var jobs = _context.JobPositions.OrderBy(x => x.RowOrder).Where(x => teamJobs.Contains(x.Id))
                        .Select(x => JobPositionLookupModel.Create(x)).ToList();

                return new JobPositionListViewModel{JobPositions = jobs, ParentId = request.DepartmentTeamId};
            }

        }
    }
}
