using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.AddRemoveUserToProject
{
    public class ProjectAddOrRemoveUserCommandHandler : IRequestHandler<ProjectAddOrRemoveUserCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public ProjectAddOrRemoveUserCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ProjectAddOrRemoveUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userName = _context.ApplicationUsers
                .Find(Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value)).UserName;
            var project = await _context.Projects.FindAsync(request.ProjectId);


            if (request.IncludeUser)
            {
                var exists = _context.ProjectMembers.First(x => x.UserId == request.ApplicationUserId);
                if (exists == null)
                {
                    _context.ProjectMembers.Add(new ProjectMember()
                    {
                        ProjectId = request.ProjectId,
                        UserId = request.ApplicationUserId
                    });
                    await _context.SaveChangesAsync(cancellationToken);

                    var userEmail = _context.ApplicationUsers.Find(request.ApplicationUserId).Email;
                    await _mediator.Publish(new UserAddedRemovedToProject()
                    {
                        PrimaryEntityId = project.Id,
                        GroupEntityName = project.Name,
                        Message = "You were added to the team who works on this project.",
                        UserName = user.Identity.Name,
                        Recipients = new List<string> { userEmail }

                    }, cancellationToken);
                }
            }
            else
            {
                var exists = _context.ProjectMembers.First(x => x.UserId == request.ApplicationUserId);
                if (exists != null)
                {
                    _context.ProjectMembers.Remove(exists);
                    await _context.SaveChangesAsync(cancellationToken);

                    var userEmail = _context.ApplicationUsers.Find(request.ApplicationUserId).Email;
                    await _mediator.Publish(new UserAddedRemovedToProject()
                    {
                        PrimaryEntityId = project.Id,
                        GroupEntityName = project.Name,
                        Message = "You were removed from the team who works on this project",
                        UserName = user.Identity.Name,
                        Recipients = new List<string> { userEmail }
                    }, cancellationToken);
                }

            }
            return Unit.Value;
        }
    }
}
