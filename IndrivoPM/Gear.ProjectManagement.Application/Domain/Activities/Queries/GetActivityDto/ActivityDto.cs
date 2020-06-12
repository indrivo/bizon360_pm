using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDto
{
    public class ActivityDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ActivityPriority Priority { get; set; }

        public string ProjectName { get; set; }

        public Guid ProjectId { get; set; }

        public int Number { get; set; }

        public static Expression<Func<Activity, ActivityDto>> Projection
        {
            get
            {
                return activity => new ActivityDto
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    Priority = activity.ActivityPriority,
                    ProjectId = activity.ProjectId,
                    ProjectName = activity.Project.Name,
                    Number = activity.Number
                };
            }
        }

        public static ActivityDto Create(Activity activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
