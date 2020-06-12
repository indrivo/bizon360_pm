using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport
{
    public class GetProjectGroupsByFiltersQueryHandler : IRequestHandler<GetProjectGroupsByFiltersQuery,
        ProjectGroupFilteredReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetProjectGroupsByFiltersQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectGroupFilteredReportListViewModel> Handle(GetProjectGroupsByFiltersQuery request,
            CancellationToken cancellationToken)
        {
            var filteredProjects = _context.Projects
                .Include(x => x.ProjectGroup)
                .Where(x => request.ProjectStatuses.Contains(x.Status));

            var filteredActivities = _context.Activities
                .Include(x => x.ActivityType)
                .Where(x => request.ActivityTypeIds.Contains(x.ActivityTypeId));

            var filteredAssignees = _context.ActivityAssignees
                .Include(x => x.User)
                .Where(x => request.AssigneeIds.Contains(x.UserId));

            var filteredLoggedTimes = _context.LoggedTimes
                .Where(x => x.DateOfWork >= request.StartDate && x.DateOfWork <= request.DueDate);

            var queryResult = (from loggedTime in filteredLoggedTimes
                    join empAss in filteredAssignees on loggedTime.UserId equals empAss.UserId
                    join activity in filteredActivities on empAss.ActivityId equals activity.Id
                    join project in filteredProjects on activity.ProjectId equals project.Id
                    where loggedTime.ActivityId == activity.Id
                    select new ProjectLogsDto
                    {
                        AssigneeName = $"{empAss.User.FirstName} {empAss.User.LastName}",
                        Date = loggedTime.DateOfWork,
                        LoggedTime = loggedTime.Time,
                        ProjectGroupName = project.ProjectGroup.Name,
                        ProjectName = project.Name,
                        AssigneeId = empAss.UserId,
                        ProjectGroupId = project.ProjectGroupId,
                        ProjectId = project.Id,
                        LoggedTimeId = loggedTime.Id
                    })
                .OrderBy(x => x.ProjectGroupId)
                .ThenBy(x => x.ProjectId)
                .ThenBy(x => x.AssigneeId)
                .Distinct();

            return await ProjectGroupFilteredReportListViewModel.Create(queryResult, request.IntervalType, request.StartDate, request.DueDate, cancellationToken);
        }
    }
}
