using Gear.Domain.PmEntities.Enums;

namespace Gear.Domain.ReportEntities.FilterEntities
{
    public class UserActivityStatusFilter<TUserIdType> : BaseFilter<TUserIdType>
    {
        public ActivityStatus ActivityStatus { get; set; }
    }
}
