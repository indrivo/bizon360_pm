using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs
{
    public class ProjectGroupDto
    {
        public Guid AssigneeId { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectGroupName { get; set; }

        public string AssigneeName { get; set; }

        public DateTime Date { get; set; }

        public float LoggedTime { get; set; }

        public static Expression<Func<ProjectLogsDto, ProjectGroupDto>> Projection
        {
            get
            {
                return projectGroup => new ProjectGroupDto
                {
                    AssigneeId = projectGroup.AssigneeId,
                    ProjectId = projectGroup.ProjectId,
                    ProjectName = projectGroup.ProjectName,
                    ProjectGroupName = projectGroup.ProjectGroupName,
                    AssigneeName = projectGroup.AssigneeName,
                    Date = projectGroup.Date,
                    LoggedTime = projectGroup.LoggedTime
                };
            }
        }

        public static ProjectGroupDto Create(ProjectLogsDto loggedTime)
        {
            return Projection.Compile().Invoke(loggedTime);
        }
    }
}
