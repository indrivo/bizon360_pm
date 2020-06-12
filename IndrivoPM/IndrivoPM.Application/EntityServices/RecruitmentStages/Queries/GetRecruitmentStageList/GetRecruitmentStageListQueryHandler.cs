using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList
{
    public class GetRecruitmentStageListQueryHandler : IRequestHandler<GetRecruitmentStageListQuery, RecruitmentStageListViewModel>
    {
        private readonly IGearContext _context;

        public GetRecruitmentStageListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<RecruitmentStageListViewModel> Handle(GetRecruitmentStageListQuery request, CancellationToken cancellationToken)
        {
            var stages = await _context.RecruitmentStages.OrderBy(x => x.Name)
                .Include(x => x.Candidates)
                .Select(x => RecruitmentStageLookUpModel.Create(x)).ToListAsync(cancellationToken);

            return new RecruitmentStageListViewModel
            {
                RecruitmentStages = stages
            };
        }
    }
}
