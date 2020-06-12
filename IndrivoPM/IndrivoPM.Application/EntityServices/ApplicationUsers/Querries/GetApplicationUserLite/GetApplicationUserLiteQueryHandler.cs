using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserLite
{
    public class GetApplicationUserLiteQueryHandler : IRequestHandler<GetApplicationUserLiteQuery, ApplicationUserLiteModel>
    {
        private readonly IGearContext _context;

        public GetApplicationUserLiteQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUserLiteModel> Handle(GetApplicationUserLiteQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ApplicationUsers.FindAsync(request.Id);

            if (entity == null) throw new NotFoundException(nameof(ApplicationUser), request.Id);

            return ApplicationUserLiteModel.Create(entity);
        }
    }
}
