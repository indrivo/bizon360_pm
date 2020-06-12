using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.DateTimeExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport
{
    public class GetEmployeeFiltersReportQueryHandler : IRequestHandler<GetEmployeeFiltersReportQuery, EmployeeFilteredReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetEmployeeFiltersReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<EmployeeFilteredReportListViewModel> Handle(GetEmployeeFiltersReportQuery request, CancellationToken cancellationToken)
        {
            var filteredAssignees = _context.ApplicationUsers
                .Where(x => request.UserIds.Contains(x.Id));

            var filteredActivities = _context.Activities
                .Include(x => x.ActivityType)
                .Where(x => request.ProjectIds.Contains(x.ProjectId));

            var filteredLoggedTimes = _context.LoggedTimes
                .Where(x => x.DateOfWork.IsInRangeInclusive(request.StartDate, request.DueDate));

            var filteredProjects = _context.Projects;

            var activityAssignees = _context.ActivityAssignees;

            var queryResult = (from loggedTime in filteredLoggedTimes
                    join activityAssignee in activityAssignees on loggedTime.UserId equals activityAssignee.UserId
                    join assignee in filteredAssignees on activityAssignee.UserId equals assignee.Id
                    join activity in filteredActivities on loggedTime.ActivityId equals activity.Id
                    join project in filteredProjects on activity.ProjectId equals project.Id
                    select new AssigneeLogsFilteredDto
                    {
                        ProjectId = activity.ProjectId,
                        ActivityId = activity.Id,
                        ActivityName = activity.Name,
                        ActivityStatus = activity.ActivityStatus,
                        AssigneeId = assignee.Id,
                        AssigneeName = $"{assignee.FirstName} {assignee.LastName}",
                        EstimatedTime = activity.EstimatedHours ?? 0f,
                        LoggedTime = loggedTime.Time,
                        ProjectName = project.Name,
                        LoggedTimeId = loggedTime.Id,
                        CreateTime = activity.CreatedTime
                    })
                .Distinct()
                .OrderBy(x => x.AssigneeId);

            return await EmployeeFilteredReportListViewModel.Create(queryResult, cancellationToken);
        }
    }
}
