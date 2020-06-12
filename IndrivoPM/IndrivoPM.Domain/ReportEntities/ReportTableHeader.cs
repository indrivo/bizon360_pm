using System;
using Gear.Domain.AppEntities;

namespace Gear.Domain.ReportEntities
{
    public class ReportTableHeader<TUserIdType>
    {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public Guid TableHeaderId { get; set; }

        public bool Active { get; set; }

        #region Navigation Properties
        public Report<TUserIdType> Report { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public TableHeader<TUserIdType> TableHeader { get; set; }
        #endregion
    }
}
