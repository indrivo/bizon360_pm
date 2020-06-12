using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Enums;
using Gear.Domain.ReportEntities;
using Gear.Domain.ReportEntities.Enums;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportDetails.Models;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportList
{
    public class UserReportModel
    {
        public Guid ReportId { get; set; }

        public string ReportName { get; set; }

        public IList<TableHeaderModel> TableHeaders { get; set; }
            = new List<TableHeaderModel>();

        public IList<FilterType> AllowedFilters { get; set; }
            = new List<FilterType>();

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public IList<ActivityStatus> ActivityStatusFilters { get; set; }
            = new List<ActivityStatus>();

        public IList<ProjectStatus> ProjectStatusFilters { get; set; }
            = new List<ProjectStatus>();

        public IDictionary<FilterType, List<Guid>> UserGuidFilters { get; set; }
            = new Dictionary<FilterType, List<Guid>>();

        public static Expression<Func<UserReport<Guid>, UserReportModel>> Projection
        {
            get
            {
                return userReport => new UserReportModel
                {
                    ReportId = userReport.ReportId,
                    ReportName = userReport.Report != null
                        ? userReport.Report.Name ?? string.Empty
                        : string.Empty,
                    AllowedFilters = userReport.Report != null && userReport.Report.AllowedFiltersByUser != null &&
                                     userReport.Report.AllowedFiltersByUser.Any(x => x.Active)
                        ? userReport.Report.AllowedFiltersByUser.Select(x => x.FilterType).ToList()
                        : new List<FilterType>(),
                    StartDate = userReport.Report != null && userReport.Report.UserDateFilters != null &&
                                userReport.Report.UserDateFilters.Any(x =>
                                    x.ReportId == userReport.ReportId && x.FilterType == FilterType.StartDate)
                        ? userReport.Report.UserDateFilters.FirstOrDefault(x => x.ReportId == userReport.ReportId
                                                                                && x.FilterType == FilterType.StartDate).Date
                        : DateTime.MinValue,
                    DueDate = userReport.Report != null && userReport.Report.UserDateFilters != null &&
                              userReport.Report.UserDateFilters.Any(x =>
                                  x.ReportId == userReport.ReportId && x.FilterType == FilterType.DueDate)
                        ? userReport.Report.UserDateFilters.FirstOrDefault(x => x.ReportId == userReport.ReportId
                                                                                && x.FilterType == FilterType.DueDate).Date
                        : DateTime.MinValue,
                    ActivityStatusFilters = userReport.Report != null &&
                                            userReport.Report.ActivityStatusFilters != null &&
                                            userReport.Report.ActivityStatusFilters.Any()
                        ? userReport.Report.ActivityStatusFilters.Select(x => x.ActivityStatus).ToList()
                        : new List<ActivityStatus>(),
                    ProjectStatusFilters = userReport.Report != null &&
                                           userReport.Report.UserProjectStatusFilters != null &&
                                           userReport.Report.UserProjectStatusFilters.Any()
                        ? userReport.Report.UserProjectStatusFilters.Select(x => x.ProjectStatus).ToList()
                        : new List<ProjectStatus>(),
                    UserGuidFilters = userReport.Report != null && userReport.Report.UserGuidFilters != null && userReport.Report.UserGuidFilters.Any()
                        ? userReport.Report.UserGuidFilters
                            .GroupBy(x => x.FilterType, x => x.EntityId)
                            .ToDictionary(x => x.Key, x => x.ToList())
                        : new Dictionary<FilterType, List<Guid>>()
                };
            }
        }

        public static UserReportModel Create(UserReport<Guid> userReport)
        {
            return Projection.Compile().Invoke(userReport);
        }
    }
}
