using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport
{
    public class GetOverdueTasksFilteredReportQueryHandler : IRequestHandler<GetOverdueTasksFilteredReportQuery,
        OverdueTasksFilteredReportViewModel>
    {
        private readonly IGearContext _context;

        public GetOverdueTasksFilteredReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<OverdueTasksFilteredReportViewModel> Handle(GetOverdueTasksFilteredReportQuery request, CancellationToken cancellationToken)
        {
            var filteredActivities = _context.Activities
                .Include(x => x.Project)
                .ThenInclude(x => x.ProjectGroup)
                .Where(x => request.ProjectIds.Contains(x.ProjectId))
                .Where(x =>
                    !(x.ActivityStatus == ActivityStatus.Completed || x.ActivityStatus == ActivityStatus.Refused 
                                                                   || x.ActivityStatus == ActivityStatus.Tested)
                    && ((!x.DueDate.HasValue && request.AllowNullDueDateAsDeadline) 
                        || (x.DueDate.HasValue && (int) (DateTime.Now - x.DueDate.Value).TotalDays > 0)));

            var filteredAssignees = _context.ActivityAssignees
                .Include(x => x.Activity)
                .Include(x => x.User)
                .Where(x => request.AssigneeIds.Contains(x.UserId));

            var filteredLoggedTimes = _context.LoggedTimes
                .Where(x => x.DateOfWork >= request.StartDate && x.DateOfWork <= request.DueDate);

            var queryResult = (from activity in filteredActivities
                    join assignee in filteredAssignees on activity.Id equals assignee.ActivityId
                    join loggedTime in filteredLoggedTimes on assignee.UserId equals loggedTime.UserId
                    select new OverdueResultDto
                    {
                        ProjectGroupId = activity.Project.ProjectGroupId,
                        ActivityId = activity.Id,
                        ActivityName = activity.Name,
                        AssigneeId = assignee.UserId,
                        AssigneeName = $"{assignee.User.FirstName} {assignee.User.LastName}",
                        Deadline = activity.DueDate.Value,
                        ProjectGroupName = activity.Project.ProjectGroup.Name,
                        ProjectId = activity.ProjectId,
                        ProjectName = activity.Project.Name
                    })
                .Distinct()
                .OrderBy(x => x.ProjectGroupId)
                .ThenBy(x => x.Deadline.Value)
                .ThenBy(x => x.ProjectId)
                .ThenBy(x => x.ActivityId)
                .ThenBy(x => x.AssigneeId);

            return await OverdueTasksFilteredReportViewModel.Create(queryResult, cancellationToken);
        }
    }
}
