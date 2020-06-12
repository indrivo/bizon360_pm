using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentDetails
{
    public class GetMainCommentDetailQueryHandler : IRequestHandler<GetMainCommentDetailQuery, MainCommentDetailModel>
    {
        private readonly IGearContext _context;

        public GetMainCommentDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<MainCommentDetailModel> Handle(GetMainCommentDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.MainComments.FindAsync(request.Id);

            if (entity == null) throw new NotFoundException(nameof(MainComment), request.Id);

            return MainCommentDetailModel.Create(entity);
        }
    }
}
