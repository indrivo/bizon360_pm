using System;
using Gear.Domain.ReportEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations
{
    public class ReportConfig : IEntityTypeConfiguration<Report<Guid>>
    {
        public void Configure(EntityTypeBuilder<Report<Guid>> builder)
        {
            builder.HasKey(key => key.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}
