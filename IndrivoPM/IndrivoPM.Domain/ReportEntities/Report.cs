using System;
using System.Collections.Generic;
using Gear.Domain.ReportEntities.FilterEntities;

namespace Gear.Domain.ReportEntities
{
    public class Report<TUserIdType>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        #region Navigation Properties
        public ICollection<UserActivityStatusFilter<TUserIdType>> ActivityStatusFilters { get; set; }

        public ICollection<UserDateFilter<TUserIdType>> UserDateFilters { get; set; }

        public ICollection<UserGuidFilter<TUserIdType>> UserGuidFilters { get; set; }

        public ICollection<UserProjectStatusFilter<TUserIdType>> UserProjectStatusFilters { get; set; }

        public ICollection<ReportFilter<TUserIdType>> AllowedFiltersByUser { get; set; }

        public ICollection<ReportTableHeader<TUserIdType>> ReportTableHeaders { get; set; }

        public ICollection<UserReport<TUserIdType>> UserReports { get; set; }
        #endregion
    }
}
