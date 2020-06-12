using System;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetUsers
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float? LoggedTime { get; set; }
        public float? EstimatedTotal { get; set; }
        public int? ActivitiesCount { get; set; }
        public static Expression<Func<Project, ProjectDto>> Projection
        {
            get
            {
                return project => new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    LoggedTime = project.Activities.Any() ? project.Activities.Where(HasStatus).Select(x => x.LoggedTimes.Sum(z => z.Time)).Sum() : 0,
                    ActivitiesCount = project.Activities.Count(),
                };
            }
        }

        private static bool HasStatus(Activity a)
        {
            return a.ActivityStatus != ActivityStatus.Completed || a.ActivityStatus != ActivityStatus.Refused;
        }

        public static ProjectDto Create(Project project, Guid applicationUserId)
        {
            var projectDto = Projection.Compile().Invoke(project);

            projectDto.EstimatedTotal = project.Activities.Any()
                ? project.Activities
                    .Where(activity =>
                        HasStatus(activity) && activity.Assignees.Any(u => u.UserId == applicationUserId))
                    .Select(x => x.EstimatedHours).Sum()
                : 0;

            return projectDto;
        }
    }
}
