using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateList;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateListByStage
{
    public class GetCandidateListByStageQueryHandler : IRequestHandler<GetCandidateListByStageQuery, CandidateListViewModel>
    {
        private readonly IGearContext _context;

        public GetCandidateListByStageQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<CandidateListViewModel> Handle(GetCandidateListByStageQuery request, CancellationToken cancellationToken)
        {
            var key = request.SearchParam?.ToUpper();

            var candidates = (await _context.Candidates
                .Where(x => request.Statuses.Contains(x.CandidateStatus) &&
                            x.RecruitmentStage.Id == request.RecruitmentStageId &&
                            (key == null || key == string.Empty ||
                            (x.Name.FirstName + ' ' + x.Name.LastName).ToUpper().Contains(key)))
                .Include(x => x.RecruitmentStage).ThenInclude(xx => xx.Pipeline)
                .Include(x => x.JobPosition)
                .ToListAsync(cancellationToken))
                .Select(CandidateLookupModel.Create).ToList();

            return new CandidateListViewModel
            {
                CandidateCard = candidates
            };
        }
    }
}
