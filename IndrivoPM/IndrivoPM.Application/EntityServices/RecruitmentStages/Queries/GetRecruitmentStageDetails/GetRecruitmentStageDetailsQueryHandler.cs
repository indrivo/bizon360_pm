using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageDetails
{
    public class GetRecruitmentStageDetailsQueryHandler : IRequestHandler<GetRecruitmentStageDetailsQuery, RecruitmentStageDetailsModel>
    {
        private readonly IGearContext _context;

        public GetRecruitmentStageDetailsQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<RecruitmentStageDetailsModel> Handle(GetRecruitmentStageDetailsQuery request, CancellationToken cancellationToken)
        { 
            var recruitmentStage = (await _context.RecruitmentStages
                    .Where(x => x.Id == request.Id)
                    .Include(x => x.Candidates)
                    .ToListAsync(cancellationToken))
                .Select(x => RecruitmentStageDetailsModel.Create(x)).FirstOrDefault();

            return recruitmentStage;
        }
    }
}
