using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionDetails
{
    public class GetJobPositionDetailQueryHandler : IRequestHandler<GetJobPositionDetailQuery, JobPositionDetailModel>
    {
        private readonly IGearContext _context;

        public GetJobPositionDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }


        public async Task<JobPositionDetailModel> Handle(GetJobPositionDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = (await _context.JobPositions
                .Include(x => x.JobDepartmentTeams)
                .FirstAsync(x => x.Id == request.Id, cancellationToken));


            if (entity == null)
            {
                throw new NotFoundException(nameof(JobPosition), request.Id);
            }

            return JobPositionDetailModel.Create(entity);
        }
    }
}
