using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralSprintListByProjectReport
{
    public class SprintListGeneralReportListViewModel
    {
        public string ProjectName { get; set; }

        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public List<SprintGeneralLookupModel> Sprints { get; set; }
            = new List<SprintGeneralLookupModel>();

        public static Expression<Func<Project, SprintListGeneralReportListViewModel>> Projection
        {
            get
            {
                return project => new SprintListGeneralReportListViewModel
                    {
                        ProjectName = project.Name,
                        TotalLoggedTime = project.Activities.Any() 
                            ? project.Activities
                                .Select(a => a.LoggedTimes != null ? a.LoggedTimes.Sum(lt => lt.Time) : 0f).Sum()
                            : 0f,
                        TotalEstimatedTime = project.Activities.Sum(x => x.EstimatedHours ?? 0f),
                        Sprints = project.Sprints.Select(SprintGeneralLookupModel.Create).ToList()
                    };
            }
        }
        public static SprintListGeneralReportListViewModel Create(Project project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
