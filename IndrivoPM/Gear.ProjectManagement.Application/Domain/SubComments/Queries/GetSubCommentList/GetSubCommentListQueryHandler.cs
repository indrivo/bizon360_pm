using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Queries.GetSubCommentList
{
    public class GetSubCommentListQueryHandler : IRequestHandler<GetSubCommentListQuery, SubCommentListViewModel>
    {
        private readonly IGearContext _context;

        public GetSubCommentListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<SubCommentListViewModel> Handle(GetSubCommentListQuery request, CancellationToken cancellationToken)
        {
            return new SubCommentListViewModel
            {
                SubComments = await _context.SubComments.OrderByDescending(x => x.CreatedTime)
                    .Where(x => x.MainCommentId == request.MainCommentId)
                    .Select(x => SubCommentLookupModel.Create(x))
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
