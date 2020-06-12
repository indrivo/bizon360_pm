using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityProgress
{
    public class GetActivityProgressQueryHandler : IRequestHandler<GetActivityProgressQuery, int>
    {
        private readonly IGearContext _context;

        public GetActivityProgressQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetActivityProgressQuery request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);

            if (activity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            return activity.Progress;
        }
    }
}
