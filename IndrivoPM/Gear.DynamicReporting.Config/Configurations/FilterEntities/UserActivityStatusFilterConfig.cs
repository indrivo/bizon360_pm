using System;
using Gear.Domain.ReportEntities.FilterEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations.FilterEntities
{
    public class UserActivityStatusFilterConfig : BaseFilterConfig<UserActivityStatusFilter<Guid>>
    {
        public override void Configure(EntityTypeBuilder<UserActivityStatusFilter<Guid>> builder)
        {
            base.Configure(builder);

            builder.HasOne(one => one.Report)
                .WithMany(many => many.ActivityStatusFilters)
                .HasForeignKey(fk => fk.ReportId);

            builder.Ignore(x => x.FilterType);
        }
    }
}
