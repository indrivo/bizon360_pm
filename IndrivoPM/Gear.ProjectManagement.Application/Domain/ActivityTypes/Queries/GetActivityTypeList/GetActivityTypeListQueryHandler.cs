using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeList
{
    public class GetActivityTypeListQueryHandler : IRequestHandler<GetActivityTypeListQuery, ActivityTypeListViewModel>
    {
        private readonly IGearContext _context;

        public GetActivityTypeListQueryHandler(IGearContext context)
        {
            _context = context;
        }
        public async Task<ActivityTypeListViewModel> Handle(GetActivityTypeListQuery request, CancellationToken cancellationToken)
        {
            return new ActivityTypeListViewModel
            {
                ActivityTypes = (await  _context.ActivityTypes.OrderBy(x => x.RowOrder)
                    .Include(x => x.TrackerTypes)
                    .Include(x => x.Activities)
                    .ToListAsync(cancellationToken)).Select(x => ActivityTypeLookupModel.Create(x)).ToList()
            };
        }
    }
}
