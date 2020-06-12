using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineDetails
{
    public class GetRecruitingPipelineDetailsQueryHandler : IRequestHandler<GetRecruitingPipelineDetailsQuery, RecruitingPipelineDetailsModel>
    {
        private readonly IGearContext _context;

        public GetRecruitingPipelineDetailsQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<RecruitingPipelineDetailsModel> Handle(GetRecruitingPipelineDetailsQuery request, CancellationToken cancellationToken)
        {
            var pipeline = await _context.RecruitingPipelines
                .Include(x => x.RecruitmentStages).ThenInclude(xx => xx.Candidates)
                .Select(x => RecruitingPipelineDetailsModel.Create(x))
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return pipeline;
        }
    }
}
