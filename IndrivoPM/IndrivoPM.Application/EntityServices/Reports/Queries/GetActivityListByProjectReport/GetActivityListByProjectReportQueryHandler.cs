using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetActivityListByProjectReport
{
    public class GetActivityListByProjectReportQueryHandler : IRequestHandler<GetActivityListByProjectReportQuery, ActivityListByProjectReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetActivityListByProjectReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        // Todo: Need to optimize.
        public async Task<ActivityListByProjectReportListViewModel> Handle(GetActivityListByProjectReportQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FindAsync(request.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(nameof(Project), request.ProjectId);
            }

            var projectActivityLists = await _context.ActivityLists
                .Where(al => request.ActivityListIds.Contains(al.Id)).ToListAsync(cancellationToken);

            var existingActivityTypesIds = await _context.ActivityTypes.OrderBy(x => x.RowOrder)
                .Select(x => new {
                    Id = x.Id,
                    Abbr = x.Abbreviation,
                    RowOrder = x.RowOrder
                }).ToListAsync(cancellationToken);

            var result = new ActivityListByProjectReportListViewModel
            {
                ProjectName = project.Name ?? "Undefined",
                ActivityTypes = existingActivityTypesIds.Select(x => x.Abbr).ToList()
            };

            foreach (var projectActivityList in projectActivityLists)
            {
                var activityList = new ActivityListByProjectReportLookupModel
                {
                    ActivityListName = projectActivityList.Name
                };

                foreach (var typeId in existingActivityTypesIds)
                {
                    var activitiesByProjectAndType = await _context.Activities.Where(x => x.ProjectId == request.ProjectId &&
                            x.ActivityTypeId == typeId.Id && x.ActivityListId == projectActivityList.Id)
                        .ToListAsync(cancellationToken);

                    activityList.ActivityTypes.Add(new ActivityTypeLookupModel
                    {
                        Abbreveation = typeId.Abbr,
                        Progress = activitiesByProjectAndType.Count > 0 ? (int?)activitiesByProjectAndType.Average(x => x.Progress) : null
                    });
                }

                var activities = await _context.Activities.Where(x => x.ProjectId == request.ProjectId
                    && x.ActivityListId == projectActivityList.Id).ToListAsync();

                var nonNullProgressActivityTypeList = activityList.ActivityTypes.Where(x => x.Progress != null).ToList();
                activityList.Average = nonNullProgressActivityTypeList.Count > 0 
                    ? (int?)nonNullProgressActivityTypeList.Average(x => x.Progress)
                    : null;

                activityList.PlannedDate = activities.Count > 0 ? activities.Max(x => x.DueDate) : null;
                activityList.ActualDate = activities.Count > 0 ? (DateTime?)activities.Max(x => x.ModifyTime) : null;

                result.ActivityList.Add(activityList);
            }

            return result;
        }
    }
}
