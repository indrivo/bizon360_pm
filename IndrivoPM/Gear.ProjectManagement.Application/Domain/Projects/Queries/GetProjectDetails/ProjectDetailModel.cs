using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Domain.PmEntities.Settings;
using Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetProjectActivityTypes;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectDetails
{
    public class ProjectDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal? Budget { get; set; }

        public float? EstimatedTime { get; set; }

        public float? LoggedTime { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ProjectUrl { get; set; }

        public Guid ProjectGroupId { get; set; }

        public string ProjectGroupName { get; set; }

        public ProjectStatus Status { get; set; }

        public ActivityPriority Priority { get; set; }

        public Guid? SolutionTypeId { get; set; }

        public string SolutionType { get; set; }

        public Guid? ServiceTypeId { get; set; }

        public string ServiceType { get; set; }

        public Guid? TechnologyTypeId { get; set; }

        public string TechnologyType { get; set; }

        public Guid? ProductTypeId { get; set; }

        public string ProductType { get; set; }

        public string Description { get; set; }

        public Guid? ProjectManagerId { get; set; }

        public ApplicationUserLookupModel ProjectManager { get; set; }

        public ICollection<Guid> ProjectMemberIds { get; set; }

        public ICollection<ApplicationUserLookupModel> ProjectMembers { get; set; }

        public ProjectSettings ProjectSettings { get; set; }

        public int Progress { get; set; }

        public int Number { get; set; }

        public IList<ProjectActivityTypeModel> ProjectActivityTypes { get; set; }
            = new List<ProjectActivityTypeModel>();

        public static Expression<Func<Project, ProjectDetailModel>> Projection
        {
            get
            {
                return project => new ProjectDetailModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Budget = project.Budget,
                    EstimatedTime = project.Activities.Sum(a => a.EstimatedHours),
                    LoggedTime = project.Activities.Select(x => x.LoggedTimes.Sum(y => y.Time)).Sum(),
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    ProjectUrl = project.ProjectUrl,
                    ProjectGroupId = project.ProjectGroupId,
                    ProjectGroupName = project.ProjectGroup.Name,
                    Status = project.Status,
                    Priority = project.Priority,
                    SolutionTypeId = project.SolutionTypeId,
                    SolutionType = project.SolutionTypes != null ? project.SolutionTypes.Name : "-",
                    ServiceTypeId = project.ServiceTypeId,
                    ServiceType = project.ServiceTypes != null ? project.ServiceTypes.Name : "-",
                    TechnologyTypeId = project.TechnologyTypeId,
                    TechnologyType = project.TechnologyTypes != null ? project.TechnologyTypes.Name : "-",
                    ProductTypeId = project.ProductTypeId,
                    ProductType = project.ProductTypes != null ? project.ProductTypes.Name : "-",
                    Description = project.Description,
                    ProjectManagerId = project.ProjectManagerId,
                    ProjectManager = ApplicationUserLookupModel.Create(project.ProjectManager),
                    ProjectMemberIds = project.ProjectMembers.Select(pm => pm.UserId).ToList(),
                    ProjectMembers = project.ProjectMembers.Select(pm => ApplicationUserLookupModel.Create(pm.User)).ToList(),
                    ProjectSettings = project.ProjectSettings,
                    Progress = project.Activities.Any() ? (int) project.Activities.Average(a => a.Progress) : 0,
                    Number = project.Number ?? 0
                };
            }
        }

        public static ProjectDetailModel Create(Project project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}