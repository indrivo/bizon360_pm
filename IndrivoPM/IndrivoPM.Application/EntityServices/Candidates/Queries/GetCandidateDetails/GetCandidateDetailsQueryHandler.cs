using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateDetails
{
    public class GetCandidateDetailsQueryHandler : IRequestHandler<GetCandidateDetailsQuery, CandidateDetailsModel>
    {
        private readonly IGearContext _context;

        public GetCandidateDetailsQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<CandidateDetailsModel> Handle(GetCandidateDetailsQuery request, CancellationToken cancellationToken)
        {
            var candidate = (await _context.Candidates
                    .Include(x => x.RecruitmentStage).ThenInclude(xx => xx.Pipeline)
                    .Include(x => x.JobPosition)
                    .ToListAsync(cancellationToken))
                .Select(CandidateDetailsModel.Create).FirstOrDefault(x => x.Id == request.Id);

            return candidate;
        }
    }
}
