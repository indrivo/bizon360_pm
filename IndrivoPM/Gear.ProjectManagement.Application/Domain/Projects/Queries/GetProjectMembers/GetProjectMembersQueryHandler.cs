using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembers
{
    public class GetProjectMembersQueryHandler : IRequestHandler<GetProjectMembersQuery, ApplicationUserListViewModel>
    {
        private readonly IGearContext _context;

        public GetProjectMembersQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUserListViewModel> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.ApplicationUsers
                .Include(u => u.Activities).ThenInclude(a => a.Activity).ThenInclude(a => a.LoggedTimes)
                .Where(u => u.Projects.Any(p => p.ProjectId == request.Id))
                .ToListAsync(cancellationToken);

            var applicationUsers = users.Select(x => new ApplicationUser
            {
                Activities = x.Activities.Where(a => a.Activity.ProjectId == request.Id).ToList(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                JobPosition = x.JobPosition,
                Id = x.Id
            });
            return await Task.FromResult(new ApplicationUserListViewModel
            {
                Users = applicationUsers.Select(user => ProjectMemberLookupModel.Create(user, request.Filter)).ToList()
            });
        }
    }
}
