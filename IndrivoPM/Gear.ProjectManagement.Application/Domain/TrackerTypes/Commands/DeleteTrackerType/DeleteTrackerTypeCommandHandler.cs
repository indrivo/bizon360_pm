using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.DeleteTrackerType
{
    public class DeleteTrackerTypeCommandHandler:IRequestHandler<DeleteTrackerTypeCommand>
    {
        private readonly IGearContext _context;

        public DeleteTrackerTypeCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTrackerTypeCommand request, CancellationToken cancellationToken)
        {
            var trackers = new List<TrackerType>();
            foreach (var item in request.Ids)
            {
                var entity = await _context.TrackerTypes.FindAsync(item);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(TrackerType), item);
                }

                trackers.Add(entity);
            }



/*
            var hasLoggedTime = _context.LoggedTimes.Any(x => x.TrackerId == entity.Id);

            if (hasLoggedTime)
            {
                throw new DeleteFailureException(nameof(TrackerType), request.Id,
                    "There are existing entities associated with this tracker type.");
            }
*/

            _context.TrackerTypes.RemoveRange(trackers);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
