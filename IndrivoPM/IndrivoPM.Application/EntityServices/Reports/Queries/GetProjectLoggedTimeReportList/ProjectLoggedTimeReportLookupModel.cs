using System;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectLoggedTimeReportList
{
    public class ProjectLoggedTimeReportLookupModel
    {
        public Guid Id { get; set; }

        public string ProjectName { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public static Expression<Func<Project, ProjectLoggedTimeReportLookupModel>> Projection
        {

            get
            {
                return project => new ProjectLoggedTimeReportLookupModel
                {
                    Id = project.Id,
                    ProjectName = project.Name,
                    EstimatedTime = project.Activities.Sum(x => x.EstimatedHours ?? 0f),
                    LoggedTime = project.Activities.Select(log => new
                    {
                        loggedTime = log.LoggedTimes.Sum(x => x.Time)
                    }).Sum(x => x.loggedTime),
                    StartDate = project.StartDate ?? DateTime.MinValue,
                    DueDate = project.EndDate ?? DateTime.MinValue
                };
            }
        }

        public static ProjectLoggedTimeReportLookupModel Create(Project project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
