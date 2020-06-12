using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.DTOs
{
    public class ResultDto
    {
        public string AssigneeName { get; set; }

        public string ActivityName { get; set; }

        public string ProjectName { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public float LoggedTime { get; set; }

        public float EstimatedTime { get; set; }

        public Guid LoggedTimeId { get; set; }

        public static Expression<Func<AssigneeLogsDto, ResultDto>> Projection
        {
            get
            {
                return loggedTime => new ResultDto
                {
                    ProjectName = loggedTime.ProjectName,
                    AssigneeName = loggedTime.AssigneeName,
                    ActivityName = loggedTime.ActivityName,
                    ActivityStatus = loggedTime.ActivityStatus,
                    LoggedTime = loggedTime.LoggedTime,
                    EstimatedTime = loggedTime.EstimatedTime,
                    LoggedTimeId = loggedTime.LoggedTimeId
                };
            }
        }

        public static ResultDto Create(AssigneeLogsDto loggedTime)
        {
            return Projection.Compile().Invoke(loggedTime);
        }

    }
}
