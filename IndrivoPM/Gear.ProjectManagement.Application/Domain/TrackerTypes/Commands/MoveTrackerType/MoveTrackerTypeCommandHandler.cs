using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.MoveTrackerType
{
    public class MoveTrackerTypeCommandHandler : IRequestHandler<MoveTrackerTypeCommand>
    {
        private readonly IGearContext _context;

        public MoveTrackerTypeCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(MoveTrackerTypeCommand request, CancellationToken cancellationToken)
        {
            var checkActivity = await _context.ActivityTypes.FindAsync(request.ActivityTypeId);

            if (checkActivity == null)
            {
                throw new NotFoundException(nameof(ActivityType), request.ActivityTypeId);

            }

            foreach (var item in request.TrackersIds)
            {
                var tracker = await _context.TrackerTypes.FindAsync(item);

                if (tracker == null)
                    throw new NotFoundException(nameof(TrackerType), item);

                tracker.ActivityTypeId = request.ActivityTypeId;
                _context.TrackerTypes.Update(tracker);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
