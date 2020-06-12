using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Identity.Permissions.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Identity.Manager.Domain.Platforms.Queeries.GetAllPlatforms
{
    public class GetAllPlatformsQueryHandler : IRequestHandler<GetAllPlatformsQuery, List<string>>
    {
        private readonly IPermissionContext _context;

        public GetAllPlatformsQueryHandler(IPermissionContext context)
        {
            _context = context;
        }

        public async Task<List<string>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Platforms.Select(x => x.Name).ToListAsync(cancellationToken);
        }
    }
}
