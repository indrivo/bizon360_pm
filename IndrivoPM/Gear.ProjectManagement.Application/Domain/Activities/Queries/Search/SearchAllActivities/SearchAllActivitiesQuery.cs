using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Gear.Common.Extensions.StringExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchAllActivities
{
    /// <summary>
    /// Query class
    /// </summary>
    public class SearchAllActivitiesQuery : IRequest<ActivityListViewModel>
    {
        public string SearchValue { get; set; }
    }

    /// <summary>
    /// Request handler class
    /// </summary>
    public class SearchAllActivitiesQueryHandler : IRequestHandler<SearchAllActivitiesQuery, ActivityListViewModel>
    {
        private readonly IGearContext _context;

        public SearchAllActivitiesQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityListViewModel> Handle(SearchAllActivitiesQuery request, CancellationToken cancellationToken)
        {
            request.SearchValue = request.SearchValue.Trim();
            var id = request.SearchValue.GetCustomId();

            var activities = (await _context.Activities
                    .Include(a => a.Project)
                    .Include(a => a.ActivityType)
                    .Include(a => a.Assignees)
                    .Include(a => a.LoggedTimes)
                    .Include(a => a.Sprint).ThenInclude(s => s.Activities)
                    .Where(a => a.Name.ToLower().Contains(request.SearchValue.ToLower()) 
                            || (a.Project != null && a.Project.Name != null && a.Project.Name.ToLower().Contains(request.SearchValue.ToLower())) 
                            || a.Number == id)
                    .ToListAsync(cancellationToken))
                .Select(ActivityLookupModel.Create).ToList();

            return await Task.FromResult(new ActivityListViewModel
            {
                Activities = activities
            });
        }
    }

    /// <summary>
    /// Abstract validator class
    /// </summary>
    public class SearchAllActivitiesQueryValidator : AbstractValidator<SearchAllActivitiesQuery>
    {
        public SearchAllActivitiesQueryValidator()
        {
            RuleFor(request => request.SearchValue).NotNull().NotEmpty();
        }
    }
}
