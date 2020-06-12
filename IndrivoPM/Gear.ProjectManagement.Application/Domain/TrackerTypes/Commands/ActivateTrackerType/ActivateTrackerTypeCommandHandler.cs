using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.ActivateTrackerType
{

    public class ActivateTrackerTypeCommandHandler : IRequestHandler<ActivateTrackerTypeCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public ActivateTrackerTypeCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(ActivateTrackerTypeCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            var trackers = new List<TrackerType>();

            foreach (var item in request.Ids)
            {
                var entity = await _context.TrackerTypes.FindAsync(item);

                if (entity == null) { throw new NotFoundException(nameof(TrackerType), item); }

                if (request.Active)
                    entity.Activate();
                else
                    entity.Deactivate();

                trackers.Add(entity);
            }

            _context.TrackerTypes.UpdateRange(trackers);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
