using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetProjectActivityTypes
{
    public class GetProjectActivityTypesQueryHandler : IRequestHandler<GetProjectActivityTypesQuery, ProjectActivityTypeListViewModel>
    {
        private readonly IGearContext _context;

        public GetProjectActivityTypesQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectActivityTypeListViewModel> Handle(GetProjectActivityTypesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _context.ProjectActivityTypes
                .Include(x => x.ActivityType)
                .Where(x => x.ProjectId == request.ProjectId).ToListAsync(cancellationToken);

            return new ProjectActivityTypeListViewModel
            {
                ProjectActivityTypes = entities.Select(ProjectActivityTypeModel.Create).ToList()
            };
        }
    }
}
