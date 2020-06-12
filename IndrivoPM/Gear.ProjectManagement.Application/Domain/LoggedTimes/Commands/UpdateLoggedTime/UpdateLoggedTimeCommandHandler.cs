using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.UpdateLoggedTime
{
    public class UpdateLoggedTimeCommandHandler : IRequestHandler<UpdateLoggedTimeCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public UpdateLoggedTimeCommandHandler(IUserAccessor userAccessor, IGearContext context)
        {
            _userAccessor = userAccessor;
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateLoggedTimeCommand request, CancellationToken cancellationToken)
        {
            var loggedTime = await _context.LoggedTimes.FindAsync(request.Id);

            loggedTime.ActivityId = request.ActivityId;
            loggedTime.DateOfWork = request.DateOfWork;
            loggedTime.TrackerId = request.TrackerId;
            loggedTime.Time = request.Time;

            var activity = await _context.Activities
                .Include(a => a.LoggedTimes)
                .FirstOrDefaultAsync(a => a.Id == request.ActivityId, cancellationToken);

            if (activity != null)
            {
                var estimatedTime = activity.EstimatedHours ?? 0;
                var totalActivityLoggedTime = activity.LoggedTimes.Sum(lt => lt.Time);
                var actualPercentage = (totalActivityLoggedTime * 100) / estimatedTime;

                if (activity.StartDate == null)
                {
                    activity.StartDate = request.DateOfWork;
                }

                if (activity.ActivityStatus == ActivityStatus.New)
                {
                    activity.ActivityStatus = ActivityStatus.InProgress;
                }

                activity.Progress = (int) actualPercentage;

                _context.Activities.Update(activity);
            }

            _context.LoggedTimes.Update(loggedTime);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
