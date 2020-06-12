using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.SearchProjects
{
    #region Request class

    public class SearchProjectsQuery : IRequest<ProjectGroupSearchViewModel>
    {
        public bool HasAccessToAllEntities { get; set; }

        public string SearchValue { get; set; }
    }

    #endregion

    #region Request handler class

    public class SearchProjectsQueryHandler : IRequestHandler<SearchProjectsQuery, ProjectGroupSearchViewModel>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public SearchProjectsQueryHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<ProjectGroupSearchViewModel> Handle(SearchProjectsQuery request, CancellationToken cancellationToken)
        {
            request.SearchValue = request.SearchValue.Trim();

            var projectGroups = (await _context.ProjectGroups
                    .Where(pg => pg.Projects.Any(p => p.Name.ToLower().Contains(request.SearchValue.ToLower())))
                    .Include(p => p.Projects).ThenInclude(p => p.ProjectMembers).ThenInclude(pm => pm.User)
                    .Include(p => p.Projects).ThenInclude(p => p.Activities)
                    .OrderByDescending(pg => pg.IsDeletable).ThenBy(pg => pg.Name)
                    .ToListAsync(cancellationToken))
                .Select(pg => ProjectGroupIncludeModel.Create(pg, request.SearchValue)).ToList();

            return new ProjectGroupSearchViewModel
            {
                ProjectGroups = projectGroups
            };
        }
    }

    #endregion

    #region Request validator class

    public class SearchProjectsQueryValidator : AbstractValidator<SearchProjectsQuery>
    {
        public SearchProjectsQueryValidator()
        {
            RuleFor(request => request.SearchValue).NotNull().NotEmpty();
        }
    }

    #endregion

    #region View model

    public class ProjectGroupSearchViewModel
    {
        public ICollection<ProjectGroupIncludeModel> ProjectGroups { get; set; }
    }

    public class ProjectGroupIncludeModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int CompletedProjectsCount { get; set; }

        public int ProjectsCount { get; set; }

        public bool IsDeletable { get; set; }

        public ICollection<ProjectLookupModel> Projects { get; set; }

        public static Expression<Func<ProjectGroup, string, ProjectGroupIncludeModel>> Projection
        {
            get
            {
                return (projectGroup, searchValue) => new ProjectGroupIncludeModel
                {
                    Id = projectGroup.Id,
                    Name = projectGroup.Name,
                    CompletedProjectsCount = projectGroup.Projects.Count(p => p.Status == ProjectStatus.Completed),
                    ProjectsCount = projectGroup.Projects.Count,
                    IsDeletable = projectGroup.IsDeletable,
                    Projects = projectGroup.Projects.Where(p => p.Name.ToLower().Contains(searchValue.ToLower())).Select(ProjectLookupModel.Create).ToList()
                };
            }
        }

        public static ProjectGroupIncludeModel Create(ProjectGroup projectGroup, string searchValue)
        {
            return Projection.Compile().Invoke(projectGroup, searchValue);
        }
    }

    #endregion
}
