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

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.CreateDepartmentTeam
{
    public class CreateDepartmentTeamCommandHandler : IRequestHandler<CreateDepartmentTeamCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public CreateDepartmentTeamCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(CreateDepartmentTeamCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var departmentTeam = new DepartmentTeam
            {
                Name = request.Name,
                Abbreviation = request.Abbreviation,
                DepartmentId = request.DepartmentId,
                DepartmentTeamLeadId = request.DepartmentTeamLeadId,
                Description = request.Description
            };

            departmentTeam.CreateEnd(Guid.NewGuid(), request.Name, userId);


            if (departmentTeam.DepartmentId != null)
            {
                departmentTeam.Department = await _context.Departments.FindAsync(departmentTeam.DepartmentId);
            }

            if (request.Active)
                departmentTeam.Activate();
            else
                departmentTeam.Deactivate();

            await _context.DepartmentTeams.AddAsync(departmentTeam, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            if (request.JobPositions != null)
            {
                var jobsList = new List<JobDepartmentTeam>();

                foreach (var item in request.JobPositions)
                {
                    var jobs = new JobDepartmentTeam
                    {
                        DepartmentTeamId = departmentTeam.Id,
                        JobPositionId = item.Value
                    };
                    jobsList.Add(jobs);
                }
                await _context.JobDepartmentTeams.AddRangeAsync(jobsList, cancellationToken);
            }

            var userList = new List<UserDepartmentTeam>();

            if (request.Members != null)
            {
                foreach (var item in request.Members)
                {
                    if (item == null) continue;
                    var members = new UserDepartmentTeam
                    {
                        DeparmentTeamId = departmentTeam.Id,
                        UserId = item.Value
                    };
                    userList.Add(members);
                }
            }

            if (request.DepartmentTeamLeadId != null)
            {
                var checkLead = userList.FirstOrDefault(x => x.UserId == request.DepartmentTeamLeadId);

                if (checkLead == null)
                {
                    var memberTeam = new UserDepartmentTeam
                    {
                        UserId = request.DepartmentTeamLeadId.Value,
                        DeparmentTeamId = departmentTeam.Id
                    };

                    userList.Add(memberTeam);
                }
            }

            _context.UserDepartmentTeams.AddRange(userList);
            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            // --------------

            var notification = new DepartmentTeamCreated()
            {
                PrimaryEntityName = departmentTeam.Name,
                PrimaryEntityId = departmentTeam.Id,
                GroupEntityName = departmentTeam.Department != null ? departmentTeam.Department.Name : "No Department",
                GroupEntityId = departmentTeam.DepartmentId ?? Guid.NewGuid(),
                UserName = user.Identity.Name
            };

            if (userList.Any())
            {
                notification.Recipients = userList.Select(x => x.User.Email).ToList();
            }
            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
