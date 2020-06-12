using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.EditProjectMembers
{
    public class EditProjectMembersCommandHandler : IRequestHandler<EditProjectMembersCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public EditProjectMembersCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(EditProjectMembersCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.Projects.Include(p => p.ProjectMembers).ThenInclude(pm => pm.User)
                .FirstAsync(p => p.Id == request.Id, cancellationToken);

            

            var existingMembers = entity.ProjectMembers.Select(u => u.UserId).ToList();

            if (request.Users != null)
            {
                var viewModelMembers = request.Users.ToList();
                var membersToDelete = existingMembers.Except(viewModelMembers).ToList();
                var membersToAdd = viewModelMembers.Except(existingMembers).ToList();

                var projectMembersToRemove = membersToDelete.Select(userId => _context.ProjectMembers
                        .Where(pm => pm.ProjectId == entity.Id)
                        .SingleOrDefault(pm => pm.UserId == userId))
                    .Where(memberToRemove => memberToRemove != null).ToList();

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

            _context.Projects.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
