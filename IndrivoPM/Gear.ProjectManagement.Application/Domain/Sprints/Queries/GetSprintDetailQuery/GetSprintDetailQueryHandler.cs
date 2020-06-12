using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintDetailQuery
{
    public class GetSprintDetailQueryHandler : IRequestHandler<GetSprintDetailQuery, SprintDetailModel>
    {
        private readonly IGearContext _context;

        public GetSprintDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<SprintDetailModel> Handle(GetSprintDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Sprints.Include(s => s.Project)
                .FirstAsync(s => s.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Sprint), request.Id);
            }

            return SprintDetailModel.Create(entity);
        }
    }
}
