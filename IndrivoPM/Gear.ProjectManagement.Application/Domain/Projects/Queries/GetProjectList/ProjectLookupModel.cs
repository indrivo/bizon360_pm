using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectList
{
    public class ProjectLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ProjectUrl { get; set; }

        public ApplicationUserLookupModel ProjectManager { get; set; }

        public Guid ProjectGroupId { get; set; }

        public List<ApplicationUserLookupModel> ProjectMembers { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? Budget { get; set; }

        public ProjectStatus Status { get; set; }

        public int Progress { get; set; }

        public static Expression<Func<Project, ProjectLookupModel>> Projection
        {
            get
            {
                return project => new ProjectLookupModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    ProjectUrl = project.ProjectUrl,
                    ProjectManager = project.ProjectManager != null 
                        ? ApplicationUserLookupModel.Create(project.ProjectManager) 
                        : new ApplicationUserLookupModel(),
                    ProjectMembers = project.ProjectMembers.Any() 
                        ? project.ProjectMembers.Select(pm => ApplicationUserLookupModel.Create(pm.User)).ToList() 
                        : new List<ApplicationUserLookupModel>(),
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Budget = project.Budget,
                    ProjectGroupId = project.ProjectGroupId,
                    Status = project.Status,
                    Progress = project.Activities.Any() ? (int) project.Activities.Average(a => a.Progress) : 0
                };
            }
        }

        public static ProjectLookupModel Create(Project project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}