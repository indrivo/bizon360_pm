using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs
{
    public class AssigneeDto
    {
        public string AssigneeName { get; set; }

        public DateTime Date { get; set; }

        public float LoggedTime { get; set; }

        public static Expression<Func<ProjectDto, AssigneeDto>> Projection
        {
            get
            {
                return assignee => new AssigneeDto
                {
                    AssigneeName = assignee.AssigneeName,
                    Date = assignee.Date,
                    LoggedTime = assignee.LoggedTime
                };
            }
        }

        public static AssigneeDto Create(ProjectDto project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
