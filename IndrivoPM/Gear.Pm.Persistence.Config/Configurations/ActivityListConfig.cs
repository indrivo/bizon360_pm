using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class ActivityListConfig : BaseModelConfig<ActivityList>
    {
        public override void Configure(EntityTypeBuilder<ActivityList> builder)
        {
            base.Configure(builder);

            builder.HasMany(al => al.Activities)
                .WithOne(a => a.ActivityList)
                .HasForeignKey(a => a.ActivityListId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(s => s.Sprint)
                .WithMany(a => a.ActivityLists)
                .HasForeignKey(a => a.SprintId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(e => e.Number)
                .HasDefaultValueSql("nextval('\"ReferenceSequenceActivityLists\"')");
        }
    }
}
