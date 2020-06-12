using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentList
{
    public class GetMainCommentListQueryHandler : IRequestHandler<GetMainCommentListQuery, MainCommentListViewModel>
    {
        private readonly IGearContext _context;

        public GetMainCommentListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<MainCommentListViewModel> Handle(GetMainCommentListQuery request, CancellationToken cancellationToken)
        {
            return new MainCommentListViewModel
            {
                Comments = await _context.MainComments.OrderByDescending(x => x.CreatedTime)
                    .Include(x => x.SubComments)
                    .Where(x => x.RecordId == request.RecordId)
                    .Select(x => MainCommentLookupModel.Create(x))
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
