using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeDetails
{
    public class GetActivityTypeDetailQueryHandler : IRequestHandler<GetActivityTypeDetailQuery, ActivityTypeDetailModel>
    {
        private readonly IGearContext _context;

        public GetActivityTypeDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityTypeDetailModel> Handle(GetActivityTypeDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = (await _context.ActivityTypes
                .Include(x => x.TrackerTypes)
                .FirstAsync(x => x.Id == request.Id, cancellationToken));


            if (entity == null)
            {
                throw new NotFoundException(nameof(ActivityType), request.Id);
            }

            return ActivityTypeDetailModel.Create(entity);
        }
    }
}
