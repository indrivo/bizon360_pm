using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects
{
    public class ProjectLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ProjectStatus Status { get; set; }

        public ActivityPriority Priority { get; set; }

        public int CompletedActivitiesCount { get; set; }

        public int TotalActivitiesCount { get; set; }

        //public int Progress { get; set; }

        public ICollection<ApplicationUserLookupModel> Team { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid ProjectGroupId { get; set; }

        public static Expression<Func<Project, ProjectLookupModel>> Projection
        {
            get
            {
                return project => new ProjectLookupModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Status = project.Status,
                    Priority = project.Priority,
                    CompletedActivitiesCount = project.Activities.Count(a => a.ActivityStatus == ActivityStatus.Completed),
                    TotalActivitiesCount = project.Activities.Count,
                    //Progress = project.Activities.Any() ? (int) project.Activities.Average(a => a.Progress) : 0,
                    Team = project.ProjectMembers.Select(pm => ApplicationUserLookupModel.Create(pm.User)).ToList(),
                    DueDate = project.EndDate,
                    ProjectGroupId = project.ProjectGroupId
                };
            }
        }

        public static ProjectLookupModel Create(Project project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
