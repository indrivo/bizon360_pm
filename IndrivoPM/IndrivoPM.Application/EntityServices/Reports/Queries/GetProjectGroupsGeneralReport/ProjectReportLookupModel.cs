using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGroupsGeneralReport
{
    public class ProjectReportLookupModel
    {
        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public static Expression<Func<LoggedTime, ProjectReportLookupModel>> Projection
        {
            get
            {
                return activity => new ProjectReportLookupModel
                {
                    LoggedTime = activity.Time
                };
            }
        }

        public static ProjectReportLookupModel Create(LoggedTime activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
