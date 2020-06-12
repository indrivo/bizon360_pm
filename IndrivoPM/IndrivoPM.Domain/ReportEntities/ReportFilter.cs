using System;
using Gear.Domain.AppEntities;
using Gear.Domain.ReportEntities.Enums;

namespace Gear.Domain.ReportEntities
{
    public class ReportFilter<TUserIdType>
     {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public FilterType FilterType { get; set; }

        public bool Active { get; set; }

        #region Navigation Properties

        public Report<TUserIdType> Report { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        #endregion
    }
}
