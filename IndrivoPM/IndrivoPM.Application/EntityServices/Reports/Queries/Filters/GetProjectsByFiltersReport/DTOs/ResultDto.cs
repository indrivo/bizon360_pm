using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs
{
    public class ResultDto
    {
        public string ProjectName { get; set; }

        public string AssigneeName { get; set; }

        public string ActivityName { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public string ActivityType { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }


        public Guid ActivityPkId { get; set; }

        public DateTime CreateTime { get; set; }

        public static Expression<Func<ProjectLoggsDto, ResultDto>> Projection
        {
            get
            {
                return loggedTime => new ResultDto
                {
                    ProjectName = loggedTime.ProjectName,
                    AssigneeName = loggedTime.AssigneeName,
                    LoggedTime = loggedTime.LoggedTime,
                    ActivityType = loggedTime.ActivityType,
                    ActivityName = loggedTime.ActivityName,
                    ActivityStatus = loggedTime.ActivityStatus,
                    EstimatedTime = loggedTime.EstimatedTime,
                    CreateTime = loggedTime.CreateTime
                };
            }
        }

        public static ResultDto Create(ProjectLoggsDto loggedTime)
        {
            return Projection.Compile().Invoke(loggedTime);
        }
    }
}
