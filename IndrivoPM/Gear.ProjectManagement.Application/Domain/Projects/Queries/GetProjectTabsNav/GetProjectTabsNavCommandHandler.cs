using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectTabsNav
{
    public class GetProjectTabsNavCommandHandler : IRequestHandler<GetProjectTabsNavCommand, ProjectTabModel>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public GetProjectTabsNavCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<ProjectTabModel> Handle(GetProjectTabsNavCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var project = await _context.Projects
                .Include(p => p.ProjectSettings)
                .Include(p => p.ProjectMembers)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
                throw new NotFoundException(nameof(Project), request.Id);

            if (request.IsUserPm || project.ProjectMembers.Any(pm => pm.UserId == userId))
                return ProjectTabModel.Create(project);
            
            return null;
        }
    }
}
