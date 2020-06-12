using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Extensions.EntityComparison;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public UpdateProjectCommandHandler(IMediator mediator, IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.Projects
                .Include(p => p.ProjectMembers).ThenInclude(pm => pm.User)
                .Include(p => p.ProjectSettings)
                .FirstAsync(p => p.Id == request.Id, cancellationToken);

            entity.Name = request.Name;
            entity.ProjectGroupId = request.ProjectGroupId ?? _context.ProjectGroups.First(pg => !pg.IsDeletable).Id;
            entity.Status = request.Status;
            entity.Priority = request.Priority;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.ProjectManagerId = request.ProjectManagerId;
            entity.Description = request.Description;
            entity.ProjectUrl = request.ProjectUrl;
            entity.Budget = request.Budget;
            entity.SolutionTypeId = request.SolutionTypeId;
            entity.ServiceTypeId = request.ServiceTypeId;
            entity.TechnologyTypeId = request.TechnologyTypeId;
            entity.ProductTypeId = request.ProductTypeId;

            

            var existingMembers = entity.ProjectMembers.Select(u => u.UserId).ToList();

            if (request.ProjectMemberIds != null)
            {
                var viewModelMembers = request.ProjectMemberIds.ToList();
                var membersToDelete = existingMembers.Except(viewModelMembers).ToList();
                var membersToAdd = viewModelMembers.Except(existingMembers).ToList();

                var projectMembersToRemove = membersToDelete.Select(userId => _context.ProjectMembers
                        .Where(pt => pt.ProjectId == entity.Id)
                        .SingleOrDefault(pt => pt.UserId == userId))
                    .Where(projectMemberToRemove => projectMemberToRemove != null).ToList();

                if (projectMembersToRemove.Any())
                {
                    _context.ProjectMembers.RemoveRange(projectMembersToRemove);
                }

                if (membersToAdd.Any())
                {
                    var memberList = membersToAdd
                        .Select(userId => new ProjectMember {UserId = userId, ProjectId = entity.Id}).ToList();
                    await _context.ProjectMembers.AddRangeAsync(memberList, cancellationToken);
                }
            }
            else
            {
                entity.ProjectMembers.Clear();
            }

            var changes = _context.Projects.Include(x => x.ProjectMembers)
                .ThenInclude(x => x.User).AsNoTracking()
                .Include(x => x.ProjectManager)
                .Include(x => x.ProjectGroup)
                .Include(x => x.ProjectSettings)
                .First(x => x.Id == request.Id)
                .EnumeratePropertyDifferences(entity, _context)
                .Aggregate("", (current, change) => current + ("\n" + change));

            _context.Projects.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            try
            {
                // Notifications
                if (entity.ProjectSettings.ProjectNotificationOnUpdateCompleteArchive)
                {
                    await _mediator.Publish(new ProjectUpdated
                    {
                        PrimaryEntityId = entity.Id,
                        PrimaryEntityName = entity.Name,
                        GroupEntityId = entity.ProjectGroupId,
                        GroupEntityName = entity.ProjectGroup.Name,
                        Changes = !string.IsNullOrEmpty(changes) ? changes : " ",
                        Recipients = entity.ProjectMembers.Any() ? entity.ProjectMembers.Select(pm => pm.User.Email).ToList() : new List<string>(),
                        UserName = user.Identity.Name
                    }, cancellationToken);
                }
            }
            catch (Exception e)
            {
                // TODO:
            }
            
            return Unit.Value;
        }
    }
}