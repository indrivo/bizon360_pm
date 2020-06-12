using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignJobPosition
{
    public class AssignJobPositionCommandHandler : IRequestHandler<AssignJobPositionCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public AssignJobPositionCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(AssignJobPositionCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = await _context.DepartmentTeams.FindAsync(request.Id);

            if (entity == null) throw new NotFoundException("Not exist an entity with this id", nameof(DepartmentTeam));

            // Update many to many JobDepartmentTeams
            var currentJobs = await _context.JobDepartmentTeams.Where(x => x.DepartmentTeamId == request.Id)
                .Select(x => x.JobPositionId).ToListAsync(cancellationToken);
            var addJobs = request.JobPositionIds.Except(currentJobs).ToList();
            var removeJobs = currentJobs.Except(request.JobPositionIds).ToList();

            if (addJobs.Count != 0)
            {
                foreach (var item in addJobs)
                {
                    var teamJob = new JobDepartmentTeam {DepartmentTeamId = request.Id, JobPositionId = item};
                    _context.JobDepartmentTeams.Add(teamJob);
                }
            }

            if (removeJobs.Count != 0)
            {
                foreach (var item in removeJobs)
                {
                    var teamJob = _context.JobDepartmentTeams
                        .First(x => x.DepartmentTeamId == request.Id && x.JobPositionId == item);

                    if (teamJob != null) _context.JobDepartmentTeams.Remove(teamJob);
                }
            }

            

            _context.DepartmentTeams.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
