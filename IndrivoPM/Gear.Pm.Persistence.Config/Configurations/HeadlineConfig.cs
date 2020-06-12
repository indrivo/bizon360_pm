using Gear.Domain.PmEntities.Wiki;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class HeadlineConfig : BaseModelConfig<Headline>
    {
        public override void Configure(EntityTypeBuilder<Headline> builder)
        {
            base.Configure(builder);

            builder.HasMany(h => h.Sections)
                .WithOne(s => s.Headline)
                .HasForeignKey(s => s.HeadlineId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
