using Gear.Domain.AppEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Persistence.Configurations
{
    public class PlatformConfig : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {

            builder.HasKey(x => x.Name);


            builder.Property(x => x.Active)
                .IsRequired();


            builder.HasMany(x => x.Roles)
                .WithOne(x => x.Platform)
                .HasForeignKey(x => x.PlatformName);
        }
    }
}
