using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintListQuery
{
    public class GetSprintListQueryHandler : IRequestHandler<GetSprintListQuery, SprintListViewModel>
    {
        private readonly IGearContext _context;

        public GetSprintListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<SprintListViewModel> Handle(GetSprintListQuery request, CancellationToken cancellationToken)
        {
            return new SprintListViewModel
            {
                Sprints = (await _context.Sprints.Where(s => s.ProjectId == request.ProjectId)
                        .Include(s => s.Activities).ThenInclude(a => a.LoggedTimes)
                        .OrderBy(s => s.EndDate).ThenBy(s => s.Name).ToListAsync(cancellationToken))
                    .Select(sprint => SprintLookupModel.Create(sprint, request.Filter)).ToList()
            };
        }
    }
}
