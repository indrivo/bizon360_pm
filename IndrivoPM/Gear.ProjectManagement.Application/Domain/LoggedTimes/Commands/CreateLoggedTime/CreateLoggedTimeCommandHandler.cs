using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.CreateLoggedTime
{
    public class CreateLoggedTimeCommandHandler : IRequestHandler<CreateLoggedTimeCommand, int?>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public CreateLoggedTimeCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<int?> Handle(CreateLoggedTimeCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var loggedTime = new LoggedTime
            {
                ActivityId = request.ActivityId,
                TrackerId = request.TrackerId,
                UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value),
                Time = request.Time,
                DateOfWork = request.DateOfWork
            };

            loggedTime.CreateEnd(Guid.NewGuid(), LoggedTimeAction.Update.ToString(), 
                Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            _context.LoggedTimes.Add(loggedTime);

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

            await _context.SaveChangesAsync(cancellationToken);

            return activity?.Progress ?? 0;
        }
    }
}
