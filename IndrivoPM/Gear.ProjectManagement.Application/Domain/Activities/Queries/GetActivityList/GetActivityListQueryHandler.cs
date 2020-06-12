using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityList
{
    public class GetActivitiesForGridQueryHandler : IRequestHandler<GetActivityListQuery, ActivitiesListViewModel>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public GetActivitiesForGridQueryHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<ActivitiesListViewModel> Handle(GetActivityListQuery request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            if (request.UserIsInvolved)
            {
                var viewModel = new ActivitiesListViewModel
                {
                    Activities = (await _context.Activities.Include(a => a.LoggedTimes)
                            .Include(a => a.ActivityType)
                            .Include(a => a.Project)
                            .Include(a => a.ActivityList)
                            .Include(a => a.Assignees)
                            .Where(a => a.ProjectId == request.ProjectId && a.Assignees.Any(x => x.UserId == Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value)))
                            .ToListAsync(cancellationToken))
                        .Select(ActivityLookupModel.Create).ToList()
                };

                return viewModel;
            }
            else
            {
                var viewModel = new ActivitiesListViewModel
                {
                    Activities = (await _context.Activities.Include(x => x.LoggedTimes)
                        .Include(x => x.ActivityType)
                        .Include(x => x.Project)
                        .Include(x => x.ActivityList)
                        .Where(a => a.ProjectId == request.ProjectId).ToListAsync(cancellationToken)).Select(ActivityLookupModel.Create).ToList()
                };

                return viewModel;
            }
        }
    }
}
