using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommandHandler : IRequestHandler<CreateApplicationUserCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateApplicationUserCommandHandler(IGearContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<Unit> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                EmploymentType = request.EmploymentType,
                JobPositionId = request.JobPositionId,
                PhoneNumber = request.PhoneNumber,
                CreatedTime = DateTime.Now,
                ModifiedTime = DateTime.Now
            };

            IdentityResult result = await _userManager.CreateAsync(user, "@userPass1");
           
            if (result.Succeeded)
            {
                try
                {
                    if (request.DepartmentTeamId != Guid.Empty)
                    {
                        var userTeam = new UserDepartmentTeam
                        {
                            DeparmentTeamId = request.DepartmentTeamId,
                            UserId = user.Id
                        };

                        _context.UserDepartmentTeams.Add(userTeam);
                        await _context.SaveChangesAsync(cancellationToken);
                    }


                    if (request.RoleNames != null)
                    {
                        await _userManager.AddToRolesAsync(user, request.RoleNames);
                    }

                    if (request.Projects != null && request.Projects.Any())
                    {
                        var projectMembers = request.Projects.Select(projectId => new ProjectMember { ProjectId = projectId, UserId = user.Id });
                        await _context.ProjectMembers.AddRangeAsync(projectMembers, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"SaveChanges threw {e.GetType().Name}: {(e is DbUpdateException ? e.InnerException?.Message : e.Message)}");
                }

            }
            else
            {
                foreach (IdentityError e in result.Errors)
                {
                    throw new Exception($"IdentityError threw {e.GetType().Name}: {e.Description}");
                }
            }

            return Unit.Value;
        }

    }
}
