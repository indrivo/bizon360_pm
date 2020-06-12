using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityListDetails
{
    public class GetActivityListDetailQueryHandler : IRequestHandler<GetActivityListDetailQuery, ActivityListDetailModel>
    {
        private readonly IGearContext _context;

        public GetActivityListDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityListDetailModel> Handle(GetActivityListDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ActivityLists
                .Include(x => x.Project)
                .Include(x => x.Sprint)
                .FirstAsync(al => al.Id == request.Id, cancellationToken: cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ActivityList), request.Id);
            }

            return ActivityListDetailModel.Create(entity);
        }
    }
}
