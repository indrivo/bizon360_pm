using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageListByPipelineAndStatus
{
    public class GetRecruitmentStageListByPipelineAndStatusQueryHandler : IRequestHandler<GetRecruitmentStageListByPipelineAndStatusQuery, RecruitmentStageListViewModel>
    {
        private readonly IGearContext _context;

        public GetRecruitmentStageListByPipelineAndStatusQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<RecruitmentStageListViewModel> Handle(GetRecruitmentStageListByPipelineAndStatusQuery request, CancellationToken cancellationToken)
        {
            var stages = await _context.RecruitmentStages.OrderBy(x => x.Name)
                .Where(x => x.Pipeline.Id == request.RecruitingPipelineId)
                .Include(x => x.Candidates)
                .Select(x => RecruitmentStageLookUpModel.Create(x)).ToListAsync(cancellationToken);

            return new RecruitmentStageListViewModel
            {
                RecruitmentStages = stages
            };
        }
    }
}
