using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.StringExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesBySprint
{
    public class SearchActivitiesBySprintQueryHandler : IRequestHandler<SearchActivitiesBySprintQuery, SprintSearchViewModel>
    {
        private readonly IGearContext _context;

        public SearchActivitiesBySprintQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<SprintSearchViewModel> Handle(SearchActivitiesBySprintQuery request, CancellationToken cancellationToken)
        {
            request.SearchValue = request.SearchValue.Trim();
            var number = request.SearchValue.GetCustomId();

            var sprints = (await _context.Sprints
                    .Include(s => s.Activities).ThenInclude(a => a.LoggedTimes)
                    .Include(s => s.Activities).ThenInclude(a => a.ActivityType)
                    .Include(s => s.Activities).ThenInclude(a => a.Assignees)
                    .Include(s => s.Activities).ThenInclude(a => a.Sprint)
                    .Where(s => s.ProjectId == request.ProjectId
                                && (s.Activities.Any(a => a.Name.ToLower().Contains(request.SearchValue.ToLower())) 
                                    || s.Activities.Any(a => a.Number == number)))
                    .ToListAsync(cancellationToken))
                .Select(SprintIncludeModel.Create).ToList();

            foreach (var sprint in sprints)
            {
                sprint.Activities = sprint.Activities
                    .Where(a => a.Name.ToLower().Contains(request.SearchValue.ToLower()) || a.Number == number).ToList();
            }

            return await Task.FromResult(new SprintSearchViewModel
            {
                Sprints = sprints
            });
        }
    }
}
