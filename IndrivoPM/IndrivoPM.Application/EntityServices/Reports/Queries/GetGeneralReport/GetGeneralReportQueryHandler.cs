using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.DateTimeExtensions;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralReport
{
    public class GetGeneralReportQueryHandler : IRequestHandler<GetGeneralReportQuery, GeneralReportList>
    {
        private readonly IGearContext _context;

        public GetGeneralReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<GeneralReportList> Handle(GetGeneralReportQuery request, CancellationToken cancellationToken)
        {
            var filteredUsers = from pm in _context.ProjectMembers
                join u in _context.ApplicationUsers on pm.UserId equals u.Id
                join p in _context.Projects on pm.ProjectId equals p.Id
                where request.TeamsIds.Contains(u.Id) && request.ProjectsIds.Contains(p.Id)
                select new
                {
                    ProjectGroup = p.ProjectGroup.Name,
                    ProjectName = p.Name,
                    ProjectId = p.Id,
                    UserName = u.UserName,
                    Id = u.Id
                };

            var filteredActivitiesTest = from actAs in _context.ActivityAssignees
                join u in filteredUsers on actAs.UserId equals u.Id
                join a in _context.Activities on actAs.ActivityId equals a.Id
                select new
                {
                    ProjectGroup = u.ProjectGroup,
                    ProjectName = u.ProjectName,
                    UserName = u.UserName,
                    UserId = u.Id,
                    ActivityName = a.Name,
                    ActivityId = actAs.ActivityId,
                    Estimated = a.EstimatedHours,
                    Sprint = a.Sprint.Name,
                    LoggedTimes = a.LoggedTimes,
                    ProjectId = u.ProjectId,
                    CreateTime = a.CreatedTime
                };

            var loggedTimesByPeriod = _context.LoggedTimes.Where(lt =>
                lt.DateOfWork.IsInRangeInclusive(request.StartDate, request.EndDate));


            var finalReport = await (from a in filteredActivitiesTest
                join t in loggedTimesByPeriod on a.ActivityId equals t.ActivityId
                select
                    new GeneralReportDto
                    {
                        ProjectGroup = a.ProjectGroup,
                        ProjectName = a.ProjectName,
                        UserName = a.UserName,
                        UserId = a.UserId,
                        ActivityName = a.ActivityName,
                        ActivityId = a.ActivityId,
                        Estimated = a.Estimated,
                        Sprint = a.Sprint,
                        ProjectId = a.ProjectId,
                        LoggedTime = t.Time,
                        CreateTime = a.CreateTime
                    }).ToListAsync(cancellationToken);

            var generalReportList = new GeneralReportList {GeneralReport = finalReport};

            return generalReportList;
        }
    }

    public class GeneralReportList
    {
        public ICollection<GeneralReportDto> GeneralReport { get; set; }
    }

    public class GeneralReportDto : IEquatable<GeneralReportDto>
    {
        public string ProjectGroup { get; set; }
        public string ProjectName { get; internal set; }
        public Guid ProjectId { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public string ActivityName { get; set; }
        public Guid ActivityId { get; internal set; }
        public float? Estimated { get; internal set; }
        public string Sprint { get; internal set; }
        public float LoggedTime { get; internal set; }
        public DateTime CreateTime { get; set; }

        public bool Equals(GeneralReportDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UserId.Equals(other.UserId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GeneralReportDto) obj);
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode();
        }
    }
}
