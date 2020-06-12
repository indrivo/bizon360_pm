using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.LookupModels
{
    public class ProjectTaskOverdueLookupModel
    {
        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public IList<ActivityTaskOverdueLookupModel> Activities { get; set; }

        public static IList<ActivityTaskOverdueLookupModel> SetActivities(List<ProjectOverdueDto> result)
        {
            var activities = result.GroupBy(x => x.ActivityId, ActivityOverdueDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return activities.Select(ActivityTaskOverdueLookupModel.Create).ToList();
        }

        public static Expression<Func<KeyValuePair<Guid, List<ProjectOverdueDto>>, ProjectTaskOverdueLookupModel>> Projection
        {
            get
            {
                return project => new ProjectTaskOverdueLookupModel
                {
                    ProjectId = project.Key,
                    ProjectName = project.Value[0].ProjectName,
                    Activities = ProjectTaskOverdueLookupModel.SetActivities(project.Value)
                };
            }
        }

        public static ProjectTaskOverdueLookupModel Create(KeyValuePair<Guid, List<ProjectOverdueDto>> projectGroup)
        {
            return Projection.Compile().Invoke(projectGroup);
        }
    }
}
