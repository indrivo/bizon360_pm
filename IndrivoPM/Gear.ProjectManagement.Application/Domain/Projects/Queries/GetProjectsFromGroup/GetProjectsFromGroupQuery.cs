using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsFromGroup
{
    #region Request class

    public class GetProjectsFromGroupQuery : IRequest<ProjectListViewModel>
    {
        public bool CanAccessAllEntities { get; set; }

        public Guid GroupId { get; set; }

        public ICollection<ProjectStatus> Filter { get; set; }

        public int Skip { get; set; }

        public int Size { get; set; } = 10;
    }

    #endregion

    #region Request handler class

    public class GetProjectsFromGroupQueryHandler : IRequestHandler<GetProjectsFromGroupQuery, ProjectListViewModel>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public GetProjectsFromGroupQueryHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<ProjectListViewModel> Handle(GetProjectsFromGroupQuery request, CancellationToken cancellationToken)
        {
            var projects = _context.Projects
                .Where(p => p.ProjectGroupId == request.GroupId && request.Filter.Contains(p.Status))
                .Include(p => p.Activities)
                .Include(p => p.ProjectMembers).ThenInclude(pm => pm.User)
                .Skip(request.Skip)
                .Take(request.Size);

            if (request.CanAccessAllEntities)
            {
                return await Task.FromResult(new ProjectListViewModel
                {
                    ProjectGroupId = request.GroupId,
                    Projects = projects.AsEnumerable().Select(ProjectLookupModel.Create).ToList()
                });
            }

            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            return await Task.FromResult(new ProjectListViewModel
            {
                ProjectGroupId = request.GroupId,
                Projects = projects.Where(p => p.ProjectMembers.Any(pm => pm.UserId == userId)).AsEnumerable().Select(ProjectLookupModel.Create).ToList()
            });
        }
    }

    #endregion

    #region Request validator class

    public class GetProjectsFromGroupQueryValidator : AbstractValidator<GetProjectsFromGroupQuery>
    {
        public GetProjectsFromGroupQueryValidator()
        {
            RuleFor(request => request.GroupId).NotNull().NotEmpty();
        }
    }

    #endregion

    #region View model

    public class ProjectListViewModel
    {
        public Guid ProjectGroupId { get; set; }

        public ICollection<ProjectLookupModel> Projects { get; set; }
    }
    
    #endregion
}
