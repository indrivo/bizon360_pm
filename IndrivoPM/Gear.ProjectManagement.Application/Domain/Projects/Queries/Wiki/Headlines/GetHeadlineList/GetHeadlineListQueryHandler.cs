using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList
{
    public class GetHeadlineListQueryHandler : IRequestHandler<GetHeadlineListQuery, HeadlineListViewModel>
    {
        private readonly IGearContext _context;

        public GetHeadlineListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<HeadlineListViewModel> Handle(GetHeadlineListQuery request, CancellationToken cancellationToken)
        {
            return new HeadlineListViewModel
            {
                ProjectId = request.ProjectId,
                Headlines = await _context.WikiHeadlines
                    .Include(h => h.Sections)
                    .Where(h => h.ProjectId == request.ProjectId)
                    .OrderBy(h => h.CreatedTime)
                    .Select(h => HeadlineLookupModel.Create(h))
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
