using System;
using Gear.Domain.ReportEntities.FilterEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations.FilterEntities
{
    public class UserDateFilterConfig : BaseFilterConfig<UserDateFilter<Guid>>
    {
        public override void Configure(EntityTypeBuilder<UserDateFilter<Guid>> builder)
        {
            base.Configure(builder);

            builder.HasOne(one => one.Report)
                .WithMany(many => many.UserDateFilters)
                .HasForeignKey(fk => fk.ReportId);
        }
    }
}
