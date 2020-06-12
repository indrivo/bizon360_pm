using Gear.Domain.CrmEntities.ManyToManyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Crm.Persistence.Config.Configurations.ManyToManyEntities
{
    public class DealTagMateConfig : IEntityTypeConfiguration<DealTagMates>
    {
        public void Configure(EntityTypeBuilder<DealTagMates> builder)
        {
            builder.Property(x => x.DealId)
                .IsRequired();

            builder.Property(x => x.ApplicationUserId)
                .IsRequired();

            builder
                .HasKey(dt => new { dt.DealId, dt.ApplicationUserId });
        }
    }
}
