using Gear.Sstp.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Sstp.Config.Configurations
{
    public abstract class SstpBaseModelConfig<T> : IEntityTypeConfiguration<T> where T : SstpBaseModel
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.IsDeletable).HasDefaultValue(true);

            builder.Property(x => x.Active).HasDefaultValue(true);

        }
    }
}
