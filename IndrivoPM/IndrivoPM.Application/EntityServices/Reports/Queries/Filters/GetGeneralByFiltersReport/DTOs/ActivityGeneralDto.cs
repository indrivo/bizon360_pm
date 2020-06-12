using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs
{
    public class ActivityGeneralDto
    {
        public Guid AssigneeId { get; set; }


        public string ActivityName { get; set; }

        public string AssigneeName { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public DateTime CreateTime { get; set; }

        public static Expression<Func<SprintGeneralDto, ActivityGeneralDto>> Projection
        {
            get
            {
                return project => new ActivityGeneralDto
                {
                    AssigneeId = project.AssigneeId,
                    ActivityName = project.ActivityName,
                    AssigneeName = project.AssigneeName,
                    EstimatedTime = project.EstimatedTime,
                    LoggedTime = project.LoggedTime,
                    CreateTime = project.CreateTime
                };
            }
        }

        public static ActivityGeneralDto Create(SprintGeneralDto project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
