using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDto
{
    public class GetActivityDetailQueryHandler : IRequestHandler<GetActivityDtoQuery, ActivityDto>
    {
        private readonly IGearContext _context;

        public GetActivityDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityDto> Handle(GetActivityDtoQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities
                .Include(a => a.Project)
                .FirstAsync(a => a.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            var result = ActivityDto.Create(entity);
            return result;
        }
    }
}
