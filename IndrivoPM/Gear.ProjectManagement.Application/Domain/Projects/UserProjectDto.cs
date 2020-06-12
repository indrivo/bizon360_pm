using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity;

namespace Gear.ProjectManagement.Manager.Domain.Projects
{
    public class UserProjectDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Progress { get; set; }

        public List<ActivityLookupModel> Activities { get; set; }

        public static Expression<Func<Project, UserProjectDto>> Projection
        {
            get
            {
                return project => new UserProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Activities = project.Activities.Select(ActivityLookupModel.Create).ToList(),
                    Progress = project.Activities.Any() ? (int)project.Activities.Average(a => a.Progress) : 0
                };
            }
        }

        public static UserProjectDto Create(Project project, Guid userId)
        {
            var projection = Projection.Compile().Invoke(project);

            projection.Activities = projection.Activities.Where(a => a.Assignees.Any(u => u.Id == userId) && a.Status != ActivityStatus.Completed).ToList();

            return projection;
        }
    }
}
