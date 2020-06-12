using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs
{
    public class AssigneeFilteredDto
    {
        public Guid ActivityId { get; set; }

        public Guid ActivityPkId { get; set; }

        public string AssigneeName { get; set; }

        public string ActivityName { get; set; }

        public string ProjectName { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public float LoggedTime { get; set; }

        public float EstimatedTime { get; set; }

        public Guid LoggedTimeId { get; set; }

        public Guid ProjectId { get; set; }

        public DateTime CreateTime { get; set; }

        public static Expression<Func<AssigneeLogsFilteredDto, AssigneeFilteredDto>> Projection
        {
            get
            {
                return assignee => new AssigneeFilteredDto
                {
                    ActivityId = assignee.ActivityId,
                    ActivityPkId = assignee.ActivityId,
                    AssigneeName = assignee.AssigneeName,
                    ActivityName = assignee.ActivityName,
                    ProjectName = assignee.ProjectName,
                    ActivityStatus = assignee.ActivityStatus,
                    LoggedTime = assignee.LoggedTime,
                    EstimatedTime = assignee.EstimatedTime,
                    LoggedTimeId = assignee.LoggedTimeId,
                    ProjectId = assignee.ProjectId,
                    CreateTime = assignee.CreateTime
                };
            }
        }

        public static AssigneeFilteredDto Create(AssigneeLogsFilteredDto loggedTime)
        {
            return Projection.Compile().Invoke(loggedTime);
        }
    }
}
