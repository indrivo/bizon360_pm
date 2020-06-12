using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetProjectActivityTypes
{
    public class ProjectActivityTypeModel
    {
        public Guid ActivityTypeId { get; set; }

        public string ActivityTypeName { get; set; }

        public bool Active { get; set; }

        public static Expression<Func<ProjectActivityType, ProjectActivityTypeModel>> Projection
        {
            get
            {
                return activityType => new ProjectActivityTypeModel
                {
                    ActivityTypeId = activityType.ActivityTypeId,
                    ActivityTypeName = activityType.ActivityType.Name,
                    Active = activityType.Active
                };
            }
        }

        public static ProjectActivityTypeModel Create(ProjectActivityType activityType)
        {
            return Projection.Compile().Invoke(activityType);
        }
    }
}
