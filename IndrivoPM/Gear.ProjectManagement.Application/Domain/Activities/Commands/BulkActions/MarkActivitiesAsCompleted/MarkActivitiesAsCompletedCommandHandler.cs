﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.MarkActivitiesAsCompleted
{
    public class MarkActivitiesAsCompletedCommandHandler : IRequestHandler<MarkActivitiesAsCompletedCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public MarkActivitiesAsCompletedCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(MarkActivitiesAsCompletedCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var activities = new List<Activity>();

            foreach (var id in request.ActivitiesById)
            {
                var activity = await _context.Activities.FindAsync(id);

                if (activity == null)
                {
                    throw new NotFoundException(nameof(Activity), id);
                }

                activity.ActivityStatus = ActivityStatus.Completed;
                

                activities.Add(activity);
            }

            if (activities.Any())
            {
                _context.Activities.UpdateRange(activities);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
