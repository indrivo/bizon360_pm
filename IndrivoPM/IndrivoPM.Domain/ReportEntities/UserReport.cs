using System;
using Gear.Domain.AppEntities;

namespace Gear.Domain.ReportEntities
{
    public class UserReport<TUserIdType>
    {
        public TUserIdType UserId { get; set; }

        public Guid ReportId { get; set; }

        public bool Active { get; set; }

        #region Navigation Properties

        public ApplicationUser ApplicationUser { get; set; }

        public Report<TUserIdType> Report { get; set; }
        #endregion
    }
}
