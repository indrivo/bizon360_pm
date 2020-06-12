using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.UpdateDepartmentTeam
{
    public class UpdateDepartmentTeamCommandHandler : IRequestHandler<UpdateDepartmentTeamCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public UpdateDepartmentTeamCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(UpdateDepartmentTeamCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var departmentTeam = _context.DepartmentTeams.Find(request.Id);
            departmentTeam.Name = request.Name;
            departmentTeam.Abbreviation = request.Abbreviation;
            departmentTeam.Description = request.Description;
            departmentTeam.DepartmentId = request.DepartmentId;
            departmentTeam.DepartmentTeamLeadId = request.DepartmentTeamLeadId;

            departmentTeam.Department = await _context.Departments.FindAsync(departmentTeam.DepartmentId);

            if (request.Active)
                departmentTeam.Activate();
            else
                departmentTeam.Deactivate();

            _context.DepartmentTeams.Update(departmentTeam);

            List<Guid?> usersToAdd, usersToRemove;

            // Update members
            if (request.MembersIds != null)
            {
                var existingUsers = _context.UserDepartmentTeams.Where(x => x.DeparmentTeamId == request.Id).ToList();
                usersToAdd = request.MembersIds.Except(existingUsers.Select(x => (Guid?)x.UserId)).ToList();
                usersToRemove = existingUsers.Select(x => (Guid?) x.UserId).Except(request.MembersIds).ToList();

                // Remove Users from team
                if (usersToRemove.Count != 0)
                {
                    foreach (var item in usersToRemove)
                    {
                       var removeFromTeam = _context.UserDepartmentTeams.First(d =>
                            d.DeparmentTeamId == request.Id && d.UserId == item.Value);
                       if (removeFromTeam != null)
                       {
                            _context.UserDepartmentTeams.Remove(removeFromTeam);
                       }
                    }
                }

                // Add Users in team
                if (usersToAdd.Count != 0)
                {
                    foreach (var item in usersToAdd)
                    {
                        var addInTeam = new UserDepartmentTeam()
                        {
                            DeparmentTeamId = departmentTeam.Id,
                            UserId = item.Value
                        };
                        _context.UserDepartmentTeams.Add(addInTeam);
                    }
                }
            }



            if (request.JobPositionIds != null)
            {
                // Remove jobs
                var oldJobsList = _context.JobDepartmentTeams.Where(x => x.DepartmentTeamId == request.Id).ToList();

                _context.JobDepartmentTeams.RemoveRange(oldJobsList);


                // Insert users

                foreach (var item in request.JobPositionIds)
                {
                    var jobs = new JobDepartmentTeam
                    {
                        DepartmentTeamId = departmentTeam.Id,
                        JobPositionId = item.Value
                    };
                    _context.JobDepartmentTeams.Add(jobs);
                }

            }


            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            // -------------

            var members = _context.UserDepartmentTeams.Where(x => x.DeparmentTeamId == request.Id)
                .Select(x => x.User.Email).ToList();

            var notification = new DepartmentTeamUpdated()
            {
                PrimaryEntityName = departmentTeam.Name,
                PrimaryEntityId = departmentTeam.Id,
                GroupEntityName = departmentTeam.Department.Name,
                GroupEntityId = departmentTeam.Department.Id,
                UserName = user.Identity.Name
            };

            if (members.Any())
            {
                notification.Recipients = members;
            }
            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
