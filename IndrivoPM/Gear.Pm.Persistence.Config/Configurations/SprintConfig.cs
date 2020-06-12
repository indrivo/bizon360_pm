using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class SprintConfig : BaseModelConfig<Sprint>
    {
        public override void Configure(EntityTypeBuilder<Sprint> builder)
        {
            base.Configure(builder);

            builder.Property(s => s.StartDate)
                .IsRequired();

            builder.Property(s => s.EndDate)
                .IsRequired();

            builder.HasMany(s => s.Activities)
                .WithOne(a => a.Sprint)
                .HasForeignKey(a => a.SprintId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
