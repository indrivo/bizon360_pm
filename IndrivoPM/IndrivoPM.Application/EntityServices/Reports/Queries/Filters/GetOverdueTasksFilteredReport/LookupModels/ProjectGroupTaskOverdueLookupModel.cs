using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.LookupModels
{
    public class ProjectGroupTaskOverdueLookupModel
    {
        public string ProjectGroupName { get; set; }

        public IList<ProjectTaskOverdueLookupModel> Projects { get; set; }

        public static IList<ProjectTaskOverdueLookupModel> SetProjects(List<ProjectGroupOverdueDto> result)
        {
            var projects = result.GroupBy(x => x.ProjectId, ProjectOverdueDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return projects.Select(ProjectTaskOverdueLookupModel.Create).ToList();
        }

        public static Expression<Func<KeyValuePair<Guid, List<ProjectGroupOverdueDto>>, ProjectGroupTaskOverdueLookupModel>> Projection
        {
            get
            {
                return projectGroup => new ProjectGroupTaskOverdueLookupModel
                {
                    ProjectGroupName = projectGroup.Value[0].ProjectGroupName,
                    Projects = ProjectGroupTaskOverdueLookupModel.SetProjects(projectGroup.Value)
                };
            }
        }

        public static ProjectGroupTaskOverdueLookupModel Create(KeyValuePair<Guid, List<ProjectGroupOverdueDto>> projectGroup)
        {
            return Projection.Compile().Invoke(projectGroup);
        }
    }
}
