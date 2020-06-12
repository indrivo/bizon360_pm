using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class CheckItemConfig : BaseModelConfig<CheckItem>
    {
        public override void Configure(EntityTypeBuilder<CheckItem> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ActivityId)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();
        }
    }
}
