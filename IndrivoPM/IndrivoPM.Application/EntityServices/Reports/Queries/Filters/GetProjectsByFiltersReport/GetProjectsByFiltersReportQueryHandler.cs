using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport
{
    public class GetProjectsByFiltersReportQueryHandler : IRequestHandler<GetProjectsByFiltersReportQuery, ProjectFilteredReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetProjectsByFiltersReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectFilteredReportListViewModel> Handle(GetProjectsByFiltersReportQuery request, CancellationToken cancellationToken)
        {
            var filteredProjects = _context.Projects
                .Include(x => x.Activities)
                .ThenInclude(xx => xx.ActivityType)
                .Include(x => x.Activities)
                .ThenInclude(xx => xx.LoggedTimes)
                .Include(x => x.Activities)
                .ThenInclude(xx => xx.Assignees)
                .ThenInclude(xxx => xxx.User)
                .Where(x => request.ProjectIds.Contains(x.Id));

            var filteredActivities = _context.Activities
                .OrderBy(x => x.Project)
                .Include(x => x.Project)
                .Where(x => request.ActivityStatuses.Contains(x.ActivityStatus))
                .Where(x => request.ActivityTypes.Contains(x.ActivityType.Id));

            var filteredLoggedTimes = _context.LoggedTimes
                .Include(x => x.User)
                .Where(x => x.DateOfWork >= request.StartDate && x.DateOfWork <= request.DueDate);

            var queryResult = (from loggedTime in filteredLoggedTimes
                    join activity in filteredActivities on loggedTime.ActivityId equals activity.Id
                    join project in filteredProjects on activity.ProjectId equals project.Id
                    select new ProjectLoggsDto
                    {
                        ActivityName = activity.Name,
                        ActivityStatus = activity.ActivityStatus,
                        ActivityType = activity.ActivityType.Abbreviation,
                        AssigneeName = $"{loggedTime.User.FirstName} {loggedTime.User.LastName}",
                        EstimatedTime = activity.EstimatedHours ?? 0f,
                        LoggedTime = loggedTime.Time,
                        ProjectName = project.Name,
                        ActivityId = activity.Id,
                        ProjectId = project.Id,
                        AssigneeId = loggedTime.UserId,
                        CreateTime = activity.CreatedTime
                    })
                .Distinct()
                .OrderBy(x => x.ProjectId)
                .ThenBy(x => x.AssigneeId)
                .ThenBy(x => x.ActivityId);

            var result = await ProjectFilteredReportListViewModel.Create(queryResult, cancellationToken);

            return result;
        }
    }
}
