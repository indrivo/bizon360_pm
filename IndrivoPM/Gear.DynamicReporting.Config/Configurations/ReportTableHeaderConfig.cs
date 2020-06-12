using System;
using Gear.Domain.ReportEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations
{
    public class ReportTableHeaderConfig : IEntityTypeConfiguration<ReportTableHeader<Guid>>
    {
        public void Configure(EntityTypeBuilder<ReportTableHeader<Guid>> builder)
        {
            builder.HasKey(key => new {key.UserId, key.ReportId, key.TableHeaderId});

            builder.HasOne(one => one.ApplicationUser)
                .WithMany()
                .HasForeignKey(fk => fk.UserId);

            builder.HasOne(one => one.Report)
                .WithMany(many => many.ReportTableHeaders)
                .HasForeignKey(fk => fk.ReportId);

            builder.Property(p => p.Active)
                .HasDefaultValue(true);
        }
    }
}
