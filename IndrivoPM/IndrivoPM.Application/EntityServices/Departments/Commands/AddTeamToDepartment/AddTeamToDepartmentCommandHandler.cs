using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.AddTeamToDepartment
{
    public class AddTeamToDepartmentCommandHandler : IRequestHandler<AddTeamToDepartmentCommand>
    {
        private readonly IGearContext _context;

        public AddTeamToDepartmentCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddTeamToDepartmentCommand request, CancellationToken cancellationToken)
        {
            var checkDepartment = await _context.Departments.AnyAsync(x => x.Id == request.DepartmentId, cancellationToken);
            var existingTeams = _context.DepartmentTeams.Where(x => x.DepartmentId == request.DepartmentId).ToList();
            var notDeletableDepartment = _context.Departments.First(x => x.IsDeletable == false);


            if (request.DepartmentTeamIds == null)
            {
                foreach (var item in existingTeams)
                {
                    item.DepartmentId = notDeletableDepartment.Id;
                    _context.DepartmentTeams.Update(item);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            if (checkDepartment && request.DepartmentTeamIds != null)
            {
                var teamsToAdd = request.DepartmentTeamIds.Except(existingTeams.Select(x => x.Id)).ToList();
                var teamsToRemove = existingTeams.Select(x => x.Id).Except(request.DepartmentTeamIds).ToList();

                if (teamsToRemove.Count != 0)
                {
                    foreach (var item in teamsToRemove)
                    {
                        var teamRemove = _context.DepartmentTeams.First(x => x.Id == item);
                        teamRemove.DepartmentId = notDeletableDepartment.Id;
                        _context.DepartmentTeams.Update(teamRemove);
                    }
                }

                if (teamsToAdd.Count != 0)
                {
                    foreach (var item in teamsToAdd)
                    {
                        var teamAdd = _context.DepartmentTeams.First(x => x.Id == item);
                        teamAdd.DepartmentId = request.DepartmentId;

                        _context.DepartmentTeams.Update(teamAdd);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}
