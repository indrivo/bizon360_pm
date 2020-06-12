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

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CompleteCheckItem
{
    public class CompleteCheckItemCommandHandler : IRequestHandler<CompleteCheckItemCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public CompleteCheckItemCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CompleteCheckItemCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.CheckItems.FindAsync(request.Id);

            entity.IsCompleted = true;
            entity.ApplicationUserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var loggedTime = new LoggedTime
            {
                UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value),
                Time = request.LoggedTime,
                ActivityId = entity.ActivityId,
                TrackerId = request.TrackerTypeId,
                DateOfWork = request.DateOfWorkValue
            };

            loggedTime.CreateEnd(Guid.NewGuid(), LoggedTimeAction.Update.ToString(),
                Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            entity.LoggedTimeId = loggedTime.Id;

            await _context.LoggedTimes.AddAsync(loggedTime, cancellationToken);

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
                    activity.StartDate = request.DateOfWorkValue;
                }

                if (activity.ActivityStatus == ActivityStatus.New)
                {
                    activity.ActivityStatus = ActivityStatus.InProgress;
                }

                activity.Progress = (int)actualPercentage;

                _context.Activities.Update(activity);
            }

            _context.CheckItems.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
