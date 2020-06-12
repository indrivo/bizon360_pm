using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs
{
    public class SprintGeneralDto
    {
        public Guid ActivityId { get; set; }

        public Guid AssigneeId { get; set; }


        public string SprintName { get; set; }

        public string ActivityName { get; set; }

        public string AssigneeName { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public DateTime CreateTime { get; set; }

        public static Expression<Func<ProjectGeneralDto, SprintGeneralDto>> Projection
        {
            get
            {
                return sprint => new SprintGeneralDto
                {
                    ActivityId = sprint.ActivityId,
                    AssigneeId = sprint.AssigneeId,
                    SprintName = sprint.SprintName,
                    ActivityName = sprint.ActivityName,
                    AssigneeName = sprint.AssigneeName,
                    EstimatedTime = sprint.EstimatedTime,
                    LoggedTime = sprint.LoggedTime,
                    CreateTime = sprint.CreateTime
                };
            }
        }

        public static SprintGeneralDto Create(ProjectGeneralDto sprint)
        {
            return Projection.Compile().Invoke(sprint);
        }
    }
}
