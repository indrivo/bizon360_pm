using System;
using Gear.Domain.AppEntities;
using Gear.Domain.ReportEntities.Enums;

namespace Gear.Domain.ReportEntities
{
    public class BaseFilter<TUserIdType>
    {
        public Guid Id { get; set; }

        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public FilterType FilterType { get; set; }

        #region Navigation Properties

        public Report<TUserIdType> Report { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        #endregion
    }
}
