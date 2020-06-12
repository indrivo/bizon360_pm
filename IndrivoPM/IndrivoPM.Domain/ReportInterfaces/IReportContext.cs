using Gear.Domain.ReportEntities;
using Gear.Domain.ReportEntities.FilterEntities;
using Microsoft.EntityFrameworkCore;

namespace Gear.Domain.ReportInterfaces
{
    public interface IReportContext<TUserIdType>
    {
        /// <summary>
        /// For built in generic methods
        /// </summary>
        DbContext Instance { get; }

        DbSet<Report<TUserIdType>> Reports { get; set; }

        DbSet<ReportFilter<TUserIdType>> ReportFilters { get; set; }

        DbSet<ReportTableHeader<TUserIdType>> ReportTableHeaders { get; set; }

        DbSet<TableHeader<TUserIdType>> TableHeaders { get; set; }

        DbSet<UserReport<TUserIdType>> UserReports { get; set; }

        #region Filters
        DbSet<UserActivityStatusFilter<TUserIdType>> UserActivityStatusFilters { get; set; }

        DbSet<UserDateFilter<TUserIdType>> UserDateFilters { get; set; }

        DbSet<UserGuidFilter<TUserIdType>> UserGuidFilters { get; set; }

        DbSet<UserProjectStatusFilter<TUserIdType>> UserProjectStatusFilters { get; set; }
        #endregion

        DbSet<ServiceTimeChecker> ServiceTimeCheckers { get; set; }
    }
}
