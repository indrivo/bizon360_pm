using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListByAssignee
{
    public class GetActivityListByAssigneeQueryHandler : IRequestHandler<GetActivityListByAssigneeQuery, AssigneeListViewModel>
    {
        private readonly IGearContext _context;

        public GetActivityListByAssigneeQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<AssigneeListViewModel> Handle(GetActivityListByAssigneeQuery request, CancellationToken cancellationToken)
        {
            var projectName = _context.Projects.Find(request.ProjectId).Name;

            return new AssigneeListViewModel
            {
                Assignees = (await _context.ApplicationUsers.Where(u => u.Projects.Any(p => p.ProjectId == request.ProjectId))
                        .Include(u => u.Activities).ThenInclude(aa => aa.Activity).ThenInclude(a => a.ActivityList)
                        .Include(u => u.Activities).ThenInclude(aa => aa.Activity).ThenInclude(a => a.Sprint)
                        .Include(u => u.Activities).ThenInclude(aa => aa.Activity).ThenInclude(a => a.ActivityType)
                        .Include(u => u.Activities).ThenInclude(aa => aa.Activity).ThenInclude(a => a.LoggedTimes)
                        .Include(u => u.Activities).ThenInclude(aa => aa.Activity).ThenInclude(a => a.Assignees).ThenInclude(aa => aa.User)
                        .Include(u => u.Activities).ThenInclude(aa => aa.Activity).ThenInclude(a => a.Project).ThenInclude(p => p.ProjectMembers).ThenInclude(pm => pm.User)
                        .Include(u => u.Activities).ThenInclude(aa => aa.Activity).ThenInclude(a => a.Project).ThenInclude(p => p.ProjectGroup)
                        .OrderBy(u => u.FirstName).ToListAsync(cancellationToken))
                    .Select(u => AssigneeLookupModel.Create(u, request.ProjectId, projectName)).ToList(),
                OpenedCollapsesById = request.OpenedCollapsesById ?? new List<string>()
            };
        }
    }
}
