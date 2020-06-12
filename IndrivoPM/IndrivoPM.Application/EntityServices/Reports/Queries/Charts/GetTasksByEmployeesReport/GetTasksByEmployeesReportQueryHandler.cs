using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByEmployeesReport
{
    public class GetTasksByEmployeesReportQueryHandler : IRequestHandler<GetTasksByEmployeesReportQuery, ProjectTasksView>
    {
        private readonly IGearContext _context;

        public GetTasksByEmployeesReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectTasksView> Handle(GetTasksByEmployeesReportQuery request, CancellationToken cancellationToken)
        {
            if (!request.ShowAllData && !request.EmployeeIds.Any())
                throw new NotFoundException(nameof(GetTasksByEmployeesReportQuery), new { request.ShowAllData, request.EmployeeIds });

            var project = await _context.Projects.FindAsync(request.ProjectId);

            if (project == null)
                throw new NotFoundException(nameof(Project), request.ProjectId);

            var filteredActivities = _context.Activities
                .Where(x => x.ProjectId == request.ProjectId);

            var filteredAssignees = _context.ActivityAssignees
                .Include(x => x.User)
                .Where(x => request.ShowAllData || request.EmployeeIds.Contains(x.UserId));

            var activities = await (from activity in filteredActivities
                    join employee in filteredAssignees on activity.Id equals employee.ActivityId
                    select employee)
                .GroupBy(x => x.UserId).ToListAsync(cancellationToken);

            return new ProjectTasksView
            {
                Employees = activities.ToDictionary(x => x.Key, x => x.ToList())
                    .Select(EmployeeModel.Create).ToList()
            };
        }
    }
}
