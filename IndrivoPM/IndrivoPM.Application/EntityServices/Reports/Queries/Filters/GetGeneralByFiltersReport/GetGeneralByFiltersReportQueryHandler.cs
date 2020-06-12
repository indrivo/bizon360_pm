using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.DateTimeExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport
{
    public class GetGeneralByFiltersReportQueryHandler : IRequestHandler<GetGeneralByFiltersReportQuery, GeneralFilteredReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetGeneralByFiltersReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<GeneralFilteredReportListViewModel> Handle(GetGeneralByFiltersReportQuery request, CancellationToken cancellationToken)
        {
            var filteredUsers = from pm in _context.ProjectMembers
                                join u in _context.ApplicationUsers on pm.UserId equals u.Id
                                join p in _context.Projects on pm.ProjectId equals p.Id
                                where request.TeamsIds.Contains(u.Id) && request.ProjectsIds.Contains(p.Id)
                                select new
                                {
                                    ProjectGroupId = p.ProjectGroupId,
                                    ProjectGroup = p.ProjectGroup.Name,
                                    ProjectId = p.Id,
                                    ProjectName = p.Name,
                                    UserName = u.UserName,
                                    Id = u.Id
                                };

            var filteredActivitiesTest = from actAs in _context.ActivityAssignees
                                         join u in filteredUsers on actAs.UserId equals u.Id
                                         join a in _context.Activities on actAs.ActivityId equals a.Id
                                         select new
                                         {
                                             ProjectGroupId = u.ProjectGroupId,
                                             ProjectGroup = u.ProjectGroup,
                                             ProjectId = u.ProjectId,
                                             ProjectName = u.ProjectName,
                                             UserName = u.UserName,
                                             UserId = u.Id,
                                             ActivityName = a.Name,
                                             ActivityId = actAs.ActivityId,
                                             Estimated = a.EstimatedHours,
                                             SprintId = a.SprintId,
                                             Sprint = a.Sprint.Name,
                                             LoggedTimes = a.LoggedTimes,
                                             CreateTime = a.CreatedTime
                                         };

            var loggedTimesByPeriod = _context.LoggedTimes.Where(lt =>
                lt.DateOfWork.IsInRangeInclusive(request.StartDate, request.EndDate));


            var queryReport = (from a in filteredActivitiesTest
                join t in loggedTimesByPeriod on a.ActivityId equals t.ActivityId
                select
                    new ProjectGroupLogsDto
                    {
                        ProjectGroupId = a.ProjectGroupId,
                        ProjectGroupName = a.ProjectGroup,
                        ProjectId = a.ProjectId,
                        ProjectName = a.ProjectName,
                        AssigneeName = a.UserName,
                        AssigneeId = a.UserId,
                        ActivityName = a.ActivityName,
                        ActivityId = a.ActivityId,
                        EstimatedTime = a.Estimated ?? 0f,
                        SprintId = a.SprintId ?? Guid.Empty,
                        SprintName = a.Sprint,
                        LoggedTime = t.Time,
                        LoggedTimeId = t.Id,
                        CreateTime = a.CreateTime
                    });

            return await GeneralFilteredReportListViewModel.Create(queryReport, cancellationToken);
        }
    }
}
