using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Wiki;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Sections.GetSectionDetails
{
    public class GetSectionDetailQueryHandler : IRequestHandler<GetSectionDetailQuery, SectionLookupModel>
    {
        private readonly IGearContext _context;

        public GetSectionDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<SectionLookupModel> Handle(GetSectionDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Sections.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Section), request.Id);
            }

            return SectionLookupModel.Create(entity);
        }
    }
}
