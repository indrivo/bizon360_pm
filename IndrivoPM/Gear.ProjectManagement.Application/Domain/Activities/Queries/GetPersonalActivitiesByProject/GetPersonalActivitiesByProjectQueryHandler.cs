using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetPersonalActivitiesByProject
{
    public class GetPersonalActivitiesByProjectQueryHandler : IRequestHandler<GetPersonalActivitiesByProjectQuery, ActivityListViewModel>
    {
        private readonly IGearContext _context;

        public GetPersonalActivitiesByProjectQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityListViewModel> Handle(GetPersonalActivitiesByProjectQuery request, CancellationToken cancellationToken)
        {
            return new ActivityListViewModel
            {
                Activities = (await _context.Activities
                        .Where(a => a.Assignees.Any(aa => aa.UserId == request.UserId) && a.ActivityStatus != ActivityStatus.Completed)
                        .Include(a => a.Project)
                        .Include(a => a.ActivityType)
                        .Include(a => a.Assignees)
                        .Include(a => a.LoggedTimes)
                        .Include(a => a.Sprint)
                        .ToListAsync(cancellationToken))
                    .Select(ActivityLookupModel.Create).ToList()
                //Projects = (await _context.Projects
                //        .Where(p => p.Activities.Any(a => a.Assignees.Any(aa => aa.UserId == request.UserId)))
                //        .Include(p => p.Activities).ThenInclude(a => a.ActivityType)
                //        .Include(p => p.Activities).ThenInclude(a => a.Assignees)
                //        .Include(p => p.Activities).ThenInclude(a => a.LoggedTimes)
                //        .Include(p => p.Activities).ThenInclude(a => a.Sprint)
                //        .OrderBy(p => p.Name).ToListAsync(cancellationToken))
                //    .Select(p => UserProjectDto.Create(p, request.UserId)).ToList()
            };
        }
    }
}
