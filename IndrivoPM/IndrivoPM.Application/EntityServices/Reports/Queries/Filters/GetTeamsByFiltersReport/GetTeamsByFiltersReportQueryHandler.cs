using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport
{
    public class GetTeamsByFiltersReportQueryHandler : IRequestHandler<GetTeamsByFiltersReportQuery, TeamsFilteredReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetTeamsByFiltersReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<TeamsFilteredReportListViewModel> Handle(GetTeamsByFiltersReportQuery request, CancellationToken cancellationToken)
        {
            var filteredAssignees = _context.ApplicationUsers
                .Where(x => request.EmployeeIds.Contains(x.Id));

            var filteredActivities = _context.Activities
                .Include(x => x.ActivityType)
                .Where(x => request.ActivityStatuses.Contains(x.ActivityStatus))
                .Where(x => request.ActivityTypeIds.Contains(x.ActivityTypeId));

            var filteredLoggedTimes = _context.LoggedTimes
                .Where(x => x.DateOfWork >= request.StartDate && x.DateOfWork <= request.DueDate);

            var filteredProjects = _context.Projects
                .Where(x => request.ProjectIds.Contains(x.Id));

            var activityAssignees = _context.ActivityAssignees;

            var queryResult = (from loggedTime in filteredLoggedTimes
                    join activityAssignee in activityAssignees on loggedTime.UserId equals activityAssignee.UserId
                    join assignee in filteredAssignees on activityAssignee.UserId equals assignee.Id
                    join activity in filteredActivities on loggedTime.ActivityId equals activity.Id
                    join project in filteredProjects on activity.ProjectId equals project.Id
                    select new AssigneeLogsDto
                    {
                        ActivityId = activity.Id,
                        ActivityName = activity.Name,
                        ActivityStatus = activity.ActivityStatus,
                        AssigneeId = assignee.Id,
                        AssigneeName = $"{assignee.FirstName} {assignee.LastName}",
                        EstimatedTime = activity.EstimatedHours ?? 0f,
                        LoggedTime = loggedTime.Time,
                        ProjectName = project.Name,
                        LoggedTimeId = loggedTime.Id,
                        ProjectId = project.Id,
                        CreateTime = activity.CreatedTime
                    })
                .Distinct()
                .OrderBy(x => x.AssigneeId);

            var result = await TeamsFilteredReportListViewModel.Create(queryResult, cancellationToken);

            return result;
        }
    }
}
