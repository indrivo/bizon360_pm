using System;

namespace Gear.Domain.ReportEntities.FilterEntities
{
    public class UserDateFilter<TUserIdType> : BaseFilter<TUserIdType>
    {
        public DateTime Date { get; set; }
    }
}
