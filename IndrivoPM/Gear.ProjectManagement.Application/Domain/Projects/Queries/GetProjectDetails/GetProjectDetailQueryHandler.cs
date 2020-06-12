using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectDetails
{
    public class GetProjectDetailQueryHandler : IRequestHandler<GetProjectDetailQuery, ProjectDetailModel>
    {
        private readonly IGearContext _context;

        public GetProjectDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectDetailModel> Handle(GetProjectDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Projects
                .Include(p => p.ProjectSettings)
                .Include(p => p.ProjectManager)
                .Include(p => p.ProjectMembers).ThenInclude(x => x.User)
                .Include(p => p.Activities).ThenInclude(x => x.LoggedTimes)
                .Include(p => p.ProjectGroup)
                .Include(p => p.SolutionTypes)
                .Include(p => p.ServiceTypes)
                .Include(p => p.TechnologyTypes)
                .Include(p => p.ProductTypes)
                .FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Project), request.Id);
            }

            return ProjectDetailModel.Create(entity);
        }
    }
}