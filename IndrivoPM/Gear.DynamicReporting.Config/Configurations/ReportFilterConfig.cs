using System;
using Gear.Domain.ReportEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations
{
    public class ReportFilterConfig : IEntityTypeConfiguration<ReportFilter<Guid>>
    {
        public void Configure(EntityTypeBuilder<ReportFilter<Guid>> builder)
        {
            builder.HasKey(key => new { key.UserId, key.ReportId, key.FilterType });

            builder.HasOne(one => one.ApplicationUser)
                .WithMany()
                .HasForeignKey(fk => fk.UserId);

            builder.HasOne(one => one.Report)
                .WithMany(many => many.AllowedFiltersByUser)
                .HasForeignKey(fk => fk.ReportId);

            builder.Property(p => p.Active)
                .HasDefaultValue(true);
        }
    }
}
