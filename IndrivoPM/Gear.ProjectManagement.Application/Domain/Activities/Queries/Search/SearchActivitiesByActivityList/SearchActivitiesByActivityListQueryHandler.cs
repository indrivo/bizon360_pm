using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.StringExtensions;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByActivityList
{
    public class SearchActivitiesByActivityListQueryHandler : IRequestHandler<SearchActivitiesByActivityListQuery, ActivityListSearchViewModel>
    {
        private readonly IGearContext _context;

        public SearchActivitiesByActivityListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityListSearchViewModel> Handle(SearchActivitiesByActivityListQuery request, CancellationToken cancellationToken)
        {
            request.SearchValue = request.SearchValue.Trim();
            var number = request.SearchValue.GetCustomId();

            return new ActivityListSearchViewModel
            {
                ActivityLists = (await _context.ActivityLists
                        .Include(al => al.Activities).ThenInclude(a => a.LoggedTimes)
                        .Include(al => al.Activities).ThenInclude(a => a.ActivityType)
                        .Include(al => al.Activities).ThenInclude(a => a.Assignees)
                        .Include(al => al.Activities).ThenInclude(a => a.Sprint)
                        .Where(al => al.ProjectId == request.ProjectId
                                     && (al.Activities.Any(
                                             a => a.Name.ToLower().Contains(request.SearchValue.ToLower()))
                                         || al.Activities.Any(a => a.Number == number)))
                        .ToListAsync(cancellationToken))
                    .Select(list => ActivityListIncludeModel.Create(list, request.SearchValue, number)).ToList()
            };
           
        }
    }
}
