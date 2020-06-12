using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.DeleteCheckItem
{
    public class DeleteCheckItemCommandHandler : IRequestHandler<DeleteCheckItemCommand>
    {
        private readonly IGearContext _context;

        public DeleteCheckItemCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCheckItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.CheckItems.FindAsync(request.CheckItemId);

            if (entity.LoggedTimeId != null)
            {
                var loggedTime = await _context.LoggedTimes.FindAsync(entity.LoggedTimeId);

                var activity = await _context.Activities
                    .Include(a => a.LoggedTimes)
                    .FirstOrDefaultAsync(a => a.Id == entity.ActivityId, cancellationToken);

                if (activity != null)
                {
                    var estimatedTime = activity.EstimatedHours ?? 0;
                    var totalActivityLoggedTime = activity.LoggedTimes.Sum(lt => lt.Time) - loggedTime.Time;
                    var actualPercentage = (totalActivityLoggedTime * 100) / estimatedTime;

                    activity.Progress = (int)actualPercentage;

                    _context.Activities.Update(activity);
                }

                _context.LoggedTimes.Remove(loggedTime);
            }

            _context.CheckItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
