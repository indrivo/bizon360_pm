using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.ReportEntities;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsById
{
    public class ReportDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IDictionary<Guid, string> Users { get; set; }

        public static Expression<Func<Report<Guid>, ReportDetailModel>> Projection
        {
            get
            {
                return report => new ReportDetailModel
                {
                    Id = report.Id,
                    Name = report.Name,
                    Users = report.UserReports != null
                        ? report.UserReports.Where(x => x.ApplicationUser != null)
                            .ToDictionary(x => x.UserId, x => x.ApplicationUser.Email)
                        : new Dictionary<Guid, string>()
                };
            }
        }

        public static ReportDetailModel Create(Report<Guid> report)
        {
            return Projection.Compile().Invoke(report);
        }
    }
}
