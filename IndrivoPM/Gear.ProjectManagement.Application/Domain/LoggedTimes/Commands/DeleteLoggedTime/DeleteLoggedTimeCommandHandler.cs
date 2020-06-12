using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.DeleteLoggedTime
{
    public class DeleteLoggedTimeCommandHandler : IRequestHandler<DeleteLoggedTimeCommand>
    {
        private readonly IGearContext _context;

        public DeleteLoggedTimeCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteLoggedTimeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.LoggedTimes.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(LoggedTime), request.Id);
            }

            var relatedActivity = await _context.Activities
                .Include(a => a.LoggedTimes)
                .FirstOrDefaultAsync(a => a.Id == entity.ActivityId, cancellationToken);

            if (relatedActivity != null)
            {
                var estimatedTime = relatedActivity.EstimatedHours ?? 0;
                var totalActivityLoggedTime = relatedActivity.LoggedTimes.Sum(lt => lt.Time) - entity.Time;
                var actualPercentage = (totalActivityLoggedTime * 100) / estimatedTime;

                relatedActivity.Progress = (int) actualPercentage;

                _context.Activities.Update(relatedActivity);
            }

            var relatedCheckItem = await _context.CheckItems.FirstOrDefaultAsync(ci => ci.LoggedTimeId == request.Id, cancellationToken);
            if (relatedCheckItem != null)
            {
                relatedCheckItem.LoggedTimeId = null;
                relatedCheckItem.IsCompleted = false;
            }

            _context.LoggedTimes.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
