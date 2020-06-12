using Gear.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public abstract class BaseModelConfig<T> : IEntityTypeConfiguration<T> where T : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.CreatedTime)
                .IsRequired();

            builder.Property(e => e.ModifiedBy)
                .IsRequired();

            builder.Property(e => e.ModifyTime)
                .IsRequired();

            builder.Property(e => e.IsDeletable);
        }
    }
}
