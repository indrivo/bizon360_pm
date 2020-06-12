using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.DTOs;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.LookupModels
{
    public class LoggedTimesFilteredByTeamsLookupModel
    {
        public ActivityStatus ActivityStatus { get; set; }

        public string ProjectName { get; set; }

        public static Expression<Func<LoggedTimeDto, LoggedTimesFilteredByTeamsLookupModel>> Projection
        {
            get
            {
                return loggedTime => new LoggedTimesFilteredByTeamsLookupModel
                {
                    ProjectName = loggedTime.ProjectName,
                    ActivityStatus = loggedTime.ActivityStatus
                };
            }
        }

        public static LoggedTimesFilteredByTeamsLookupModel Create(LoggedTimeDto result)
        {
            return Projection.Compile().Invoke(result);
        }
    }
}
