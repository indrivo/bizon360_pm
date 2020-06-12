using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectSstp
{
    internal class GetProjectSstpQueryHandler : IRequestHandler<GetProjectSstpQuery, ProjectSstpModel>
    {
        private readonly IGearContext _context;

        public GetProjectSstpQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectSstpModel> Handle(GetProjectSstpQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Projects
                .Include(x => x.ServiceTypes)
                .Include(x => x.SolutionTypes)
                .Include(x => x.TechnologyTypes)
                .Include(x => x.ProductTypes)
                .FirstAsync(x => x.Id == request.ProjectId, cancellationToken);

            if (entity == null) throw new NoNullAllowedException("Not found project");

            return ProjectSstpModel.Create(entity);
        }
    }
}
