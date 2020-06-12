using System;
using Gear.Domain.ReportEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations
{
    public class UserReportConfig : IEntityTypeConfiguration<UserReport<Guid>>
    {
        public void Configure(EntityTypeBuilder<UserReport<Guid>> builder)
        {
            builder.HasKey(key => new {key.UserId, key.ReportId});

            builder.HasOne(one => one.ApplicationUser)
                .WithMany()
                .HasForeignKey(fk => fk.UserId);

            builder.HasOne(one => one.Report)
                .WithMany(many => many.UserReports)
                .HasForeignKey(fk => fk.ReportId);

            builder.Property(p => p.Active)
                .HasDefaultValue(true);
        }
    }
}
