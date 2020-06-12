using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Wiki;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineDetails
{
    public class GetHeadlineDetailQueryHandler : IRequestHandler<GetHeadlineDetailQuery, HeadlineLookupModel>
    {
        private readonly IGearContext _context;

        public GetHeadlineDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<HeadlineLookupModel> Handle(GetHeadlineDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.WikiHeadlines
                .Include(h => h.Sections)
                .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Headline), request.Id);
            }

            return HeadlineLookupModel.Create(entity);
        }
    }
}
