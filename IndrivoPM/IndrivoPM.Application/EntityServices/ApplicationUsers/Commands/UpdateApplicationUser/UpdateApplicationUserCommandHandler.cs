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

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.UpdateApplicationUser
{
    public class UpdateApplicationUserCommandHandler : IRequestHandler<UpdateApplicationUserCommand>
    {
        private readonly IGearContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateApplicationUserCommandHandler(IGearContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.ApplicationUsers.Find(request.Id);

            entity.Id = request.Id;
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Email = request.Email;
            entity.UserName = request.Email;
            entity.PhoneNumber = request.PhoneNumber;
            entity.EmploymentType = request.EmploymentType;
            entity.JobPositionId = request.JobPositionId;
            entity.Active = request.Active;
            entity.ModifiedTime = DateTime.Now;

            UpdateTeams(request);
            UpdateProjects(request);

            _context.ApplicationUsers.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }

        private void UpdateProjects(UpdateApplicationUserCommand request)
        {
            var existingProjects = _context.ProjectMembers.Where(x => x.UserId == request.Id).ToList();
           
            if (request.Projects != null)
            {
                var projectsToRemove = existingProjects.Select(x => x.ProjectId).Except(request.Projects).ToList();
                // Remove Users from project.
                if (projectsToRemove.Count != 0)
                {
                    RemoveUserFromProjects(request, projectsToRemove);
                }

                var projectsToAdd = request.Projects.Except(existingProjects.Select(x => x.ProjectId)).ToList();
                // Add User in projects.
                if (projectsToAdd.Count != 0)
                {
                    AddUserToProjects(request, projectsToAdd);
                }
            }
            else
            {
                _context.ProjectMembers.RemoveRange(existingProjects);
            }
        }

        private void UpdateTeams(UpdateApplicationUserCommand request)
        {
            var existingTeams = _context.UserDepartmentTeams.Where(x => x.UserId == request.Id).ToList();

            if (request.TeamIds != null)
            {
                var teamsToRemove = existingTeams.Select(x => x.DeparmentTeamId).Except(request.TeamIds).ToList();
                // Remove Users from team.
                if (teamsToRemove.Count != 0)
                {
                    RemoveUsersFromTeam(request, teamsToRemove);
                }

                var teamsToAdd = request.TeamIds.Except(existingTeams.Select(x => x.DeparmentTeamId)).ToList();
                // Add Users in team.
                if (teamsToAdd.Count != 0)
                {
                    AddUsersToTeam(request, teamsToAdd);
                }
            }
            else
            {
                _context.UserDepartmentTeams.RemoveRange(existingTeams);
            }
        }

        private void AddUsersToTeam(UpdateApplicationUserCommand request, List<Guid> teamsToAdd)
        {
            foreach (var item in teamsToAdd)
            {
                var addInTeam = new UserDepartmentTeam
                {
                    DeparmentTeamId = item,
                    UserId = request.Id
                };
                _context.UserDepartmentTeams.Add(addInTeam);
            }
        }
        private void AddUserToProjects(UpdateApplicationUserCommand request, List<Guid> projectsToAdd)
        {
            var projectMembers = projectsToAdd.Select(id => new ProjectMember {ProjectId = id, UserId = request.Id});
            _context.ProjectMembers.AddRange(projectMembers);
        }
        private void RemoveUsersFromTeam(UpdateApplicationUserCommand request, List<Guid> teamsToRemove)
        {
            foreach (var item in teamsToRemove)
            {
                var removeFromTeam = _context.UserDepartmentTeams.First(d => d.UserId == request.Id && d.DeparmentTeamId == item);

                if (removeFromTeam != null)
                {
                    _context.UserDepartmentTeams.Remove(removeFromTeam);
                }
            }
        }
        private void RemoveUserFromProjects(UpdateApplicationUserCommand request, List<Guid> projectsToRemove)
        {
            var projectMembers = _context.ProjectMembers
                .Where(p => p.UserId == request.Id && projectsToRemove.Contains(p.ProjectId)).ToList();

            if (projectMembers.Any())
            {
                _context.ProjectMembers.RemoveRange(projectMembers);
            }
            
        }
    }
}
