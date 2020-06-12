using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityOverview
{
    public class GetActivityOverviewQueryHandler : IRequestHandler<GetActivityOverviewQuery, ActivityOverviewModel>
    {
        private readonly IGearContext _context;

        public GetActivityOverviewQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityOverviewModel> Handle(GetActivityOverviewQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities
                .Include(a => a.LoggedTimes)
                .FirstAsync(a => a.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            var result = ActivityOverviewModel.Create(entity);
            return result;
        }
    }
}
