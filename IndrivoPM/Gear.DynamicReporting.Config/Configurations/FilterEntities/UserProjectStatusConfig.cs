using System;
using Gear.Domain.ReportEntities.FilterEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations.FilterEntities
{
    public class UserProjectStatusFilterConfig : BaseFilterConfig<UserProjectStatusFilter<Guid>>
    {
        public override void Configure(EntityTypeBuilder<UserProjectStatusFilter<Guid>> builder)
        {
            base.Configure(builder);

            builder.HasOne(one => one.Report)
                .WithMany(many => many.UserProjectStatusFilters)
                .HasForeignKey(fk => fk.ReportId);

            builder.Ignore(x => x.FilterType);
        }
    }
}
