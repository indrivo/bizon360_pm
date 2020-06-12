using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.RemoveJobPosition
{
    public class RemoveJobPositionCommandHandler : IRequestHandler<RemoveJobPositionCommand>
    {
        private readonly IGearContext _context;

        public RemoveJobPositionCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveJobPositionCommand request, CancellationToken cancellationToken)
        {
            var jobsToRemove = new List<JobDepartmentTeam>();

            foreach (var item in request.JobPositionIds)
            {
                            jobsToRemove.Add(_context.JobDepartmentTeams
                .First(x => x.DepartmentTeamId == request.DepartmentTeamId && x.JobPositionId == item)); 
            }

            _context.JobDepartmentTeams.RemoveRange(jobsToRemove);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
