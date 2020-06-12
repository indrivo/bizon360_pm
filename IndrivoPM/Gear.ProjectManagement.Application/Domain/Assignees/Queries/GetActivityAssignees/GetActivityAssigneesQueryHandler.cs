using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Assignees.Queries.GetActivityAssignees
{
    public class GetActivityAssigneesQueryHandler : IRequestHandler<GetActivityAssigneesQuery, ActivityAssigneesDto>
    {
        private readonly IGearContext _context;

        public GetActivityAssigneesQueryHandler(IGearContext context)
        {
            _context = context;
        }
        public async Task<ActivityAssigneesDto> Handle(GetActivityAssigneesQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.ApplicationUsers.Where(u => u.Activities.Any(a => a.ActivityId == request.ActivityId))
                .ToListAsync(cancellationToken: cancellationToken);

            return await Task.FromResult(new ActivityAssigneesDto
            {
                Assignees = users.Select(ActivityAssigneeDto.Create).ToList()
            });
        }


    }
}
