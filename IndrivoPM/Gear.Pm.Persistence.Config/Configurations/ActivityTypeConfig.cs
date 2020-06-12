using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class ActivityTypeConfig : BaseModelConfig<ActivityType>
    {
        public override void Configure(EntityTypeBuilder<ActivityType> builder)
        {
            base.Configure(builder);

            builder.HasMany(x => x.TrackerTypes)
                .WithOne(x => x.ActivityType)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(at => at.Activities)
                .WithOne(a => a.ActivityType)
                .HasForeignKey(a => a.ActivityTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
