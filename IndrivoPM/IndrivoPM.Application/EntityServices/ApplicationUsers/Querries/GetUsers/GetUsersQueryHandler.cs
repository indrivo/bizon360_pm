using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, UsersDto>
    {
        private readonly IGearContext _context;

        public GetUsersQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UsersDto> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return new UsersDto
            {
                Users = (
                    await _context.ApplicationUsers
                        .Where(u => u.Active)
                        .Where(u => request.UsersIds.Contains(u.Id))
                        .Include(x => x.Projects)
                            .ThenInclude(p => p.Project)
                            .ThenInclude(z => z.Activities)
                            .ThenInclude(t => t.LoggedTimes)
                        .Include(u => u.Projects)
                            .ThenInclude(up => up.Project)
                            .ThenInclude(p => p.Activities)
                            .ThenInclude(a => a.Assignees)
                        .ToListAsync(cancellationToken)
                ).Select(UserModel.Create).ToList()
            };
        }
    }
}
