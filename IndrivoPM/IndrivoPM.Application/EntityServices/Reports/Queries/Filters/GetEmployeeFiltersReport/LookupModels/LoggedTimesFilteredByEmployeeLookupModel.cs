using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.LookupModels
{
    public class LoggedTimesFilteredByEmployeeLookupModel
    {
        public float TotalLoggedTime { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<LoggedTimeFilteredDto>>, LoggedTimesFilteredByEmployeeLookupModel>> Projection
        {
            get
            {
                return loggedTime => new LoggedTimesFilteredByEmployeeLookupModel
                {
                    TotalLoggedTime = loggedTime.Value.Sum(x => x.LoggedTime)
                };
            }
        }

        public static LoggedTimesFilteredByEmployeeLookupModel Create(KeyValuePair<Guid, List<LoggedTimeFilteredDto>> activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
