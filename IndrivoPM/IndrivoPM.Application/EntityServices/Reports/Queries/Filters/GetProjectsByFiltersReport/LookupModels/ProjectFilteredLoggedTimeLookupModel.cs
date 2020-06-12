using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.LookupModels
{
    public class ProjectFilteredLoggedTimeLookupModel
    {
        public ActivityStatus ActivityStatus { get; set; }

        public string ActivityType { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public static Expression<Func<LoggedTimeDto, ProjectFilteredLoggedTimeLookupModel>> Projection
        {
            get
            {
                return loggedTime => new ProjectFilteredLoggedTimeLookupModel
                {
                    LoggedTime = loggedTime.LoggedTime,
                    ActivityType = loggedTime.ActivityType,
                    ActivityStatus = loggedTime.ActivityStatus,
                    EstimatedTime = loggedTime.EstimatedTime
                };
            }
        }

        public static ProjectFilteredLoggedTimeLookupModel Create(LoggedTimeDto result)
        {
            return Projection.Compile().Invoke(result);
        }
    }
}
