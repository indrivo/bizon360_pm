using System;

namespace Gear.Domain.ReportEntities.FilterEntities
{
    public class UserGuidFilter<TUserIdType> : BaseFilter<TUserIdType>
    {
        public Guid EntityId { get; set; }
    }
}
