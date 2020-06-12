using System;
using Gear.Domain.ReportEntities.FilterEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations.FilterEntities
{
    class UserGuidFilterConfig : BaseFilterConfig<UserGuidFilter<Guid>>
    {
        public override void Configure(EntityTypeBuilder<UserGuidFilter<Guid>> builder)
        {
            base.Configure(builder);

            builder.HasOne(one => one.Report)
                .WithMany(many => many.UserGuidFilters)
                .HasForeignKey(fk => fk.ReportId);
        }
    }
}
