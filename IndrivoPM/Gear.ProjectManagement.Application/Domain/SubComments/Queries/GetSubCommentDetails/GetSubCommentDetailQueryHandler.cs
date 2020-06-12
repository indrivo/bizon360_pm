using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Queries.GetSubCommentDetails
{
    public class GetSubCommentDetailQueryHandler : IRequestHandler<GetSubCommentDetailQuery, SubCommentDetailModel>
    {
        private readonly IGearContext _context;

        public GetSubCommentDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<SubCommentDetailModel> Handle(GetSubCommentDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.SubComments.FindAsync(request.Id);

            if (entity == null) throw new NotFoundException(nameof(SubComment), request.Id);

            return SubCommentDetailModel.Create(entity);
        }
    }
}
