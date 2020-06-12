using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupDetails
{
    public class GetProjectGroupDetailQueryHandler : IRequestHandler<GetProjectGroupDetailQuery, ProjectGroupDetailModel>
    {
        private readonly IGearContext _context;

        public GetProjectGroupDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectGroupDetailModel> Handle(GetProjectGroupDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ProjectGroups.FirstOrDefaultAsync(pg => pg.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProjectGroup), request.Id);
            }

            return ProjectGroupDetailModel.Create(entity);
        }
    }
}
