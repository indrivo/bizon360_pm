using Gear.Domain.PmEntities.Enums;

namespace Gear.Domain.ReportEntities.FilterEntities
{
    public class UserProjectStatusFilter<TUserIdType> : BaseFilter<TUserIdType>
    {
        public ProjectStatus ProjectStatus { get; set; }
    }
}
