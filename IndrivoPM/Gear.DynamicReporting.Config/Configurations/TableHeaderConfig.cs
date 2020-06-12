using System;
using Gear.Domain.ReportEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations
{
    public class TableHeaderConfig : IEntityTypeConfiguration<TableHeader<Guid>>
    {
        public void Configure(EntityTypeBuilder<TableHeader<Guid>> builder)
        {
            builder.HasKey(key => key.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.Active)
                .HasDefaultValue(true);

            builder.Property(p => p.Deletable)
                .HasDefaultValue(true);
        }
    }
}
