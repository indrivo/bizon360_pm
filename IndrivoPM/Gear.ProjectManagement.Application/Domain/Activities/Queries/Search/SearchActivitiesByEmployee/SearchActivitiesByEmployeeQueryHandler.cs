using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.StringExtensions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByEmployee
{
    public class SearchActivitiesByEmployeeQueryHandler : IRequestHandler<SearchActivitiesByEmployeeQuery, EmployeeSearchViewModel>
    {
        private readonly IGearContext _context;

        public SearchActivitiesByEmployeeQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<EmployeeSearchViewModel> Handle(SearchActivitiesByEmployeeQuery request, CancellationToken cancellationToken)
        {
            request.SearchValue = request.SearchValue.Trim();
            var number = request.SearchValue.GetCustomId();

            var users = await _context.ApplicationUsers
                .Include(u => u.Activities).ThenInclude(a => a.Activity).ThenInclude(a => a.LoggedTimes)
                .Include(u => u.Activities).ThenInclude(a => a.Activity).ThenInclude(a => a.ActivityType)
                .Include(u => u.Activities).ThenInclude(a => a.Activity).ThenInclude(a => a.Assignees)
                .Include(u => u.Activities).ThenInclude(a => a.Activity).ThenInclude(a => a.Sprint)
                .Where(u => u.Projects.Any(p => p.ProjectId == request.ProjectId)
                            && (u.Activities.Any(a =>
                                    a.Activity.Name.ToLower().Contains(request.SearchValue.ToLower()))
                                || u.Activities.Any(a => a.Activity.Number == number)))
                .ToListAsync(cancellationToken);

            var applicationUsers = users.Select(x => new ApplicationUser
            {
                Activities = x.Activities.Where(a => a.Activity.ProjectId == request.ProjectId).ToList(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                JobPosition = x.JobPosition,
                Id = x.Id
            });

            return await Task.FromResult(new EmployeeSearchViewModel
            {
                Employees = applicationUsers.Select(user => EmployeeWithActivitiesIncludeModel.Create(user, request.SearchValue, number)).ToList()
            });
        }
    }
}
