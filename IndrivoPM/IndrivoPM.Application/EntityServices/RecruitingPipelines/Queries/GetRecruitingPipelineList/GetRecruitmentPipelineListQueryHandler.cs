using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineList
{
    public class GetRecruitmentPipelineListQueryHandler : IRequestHandler<GetRecruitingPipelineListQuery, RecruitingPipelineListViewModel>
    {
        private readonly IGearContext _context;

        public GetRecruitmentPipelineListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<RecruitingPipelineListViewModel> Handle(GetRecruitingPipelineListQuery request, CancellationToken cancellationToken)
        {
            var recruitingPipelines = await _context.RecruitingPipelines.OrderBy(x => x.Name)
                .Include(x => x.RecruitmentStages).ThenInclude(xx => xx.Candidates)
                .Select(x => RecruitingPipelineLookupModel.Create(x)).ToListAsync(cancellationToken);

            return new RecruitingPipelineListViewModel
            {
                PipelineLookupModels = recruitingPipelines
            };
        }
    }
}
