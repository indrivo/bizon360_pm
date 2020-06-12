using Gear.Domain.ReportEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations
{
    public class ServiceTimeCheckerConfig : IEntityTypeConfiguration<ServiceTimeChecker>
    {
        public void Configure(EntityTypeBuilder<ServiceTimeChecker> builder)
        {
            builder.HasKey(x => x.ServiceId);

            builder.Property(x => x.ServiceName)
                .IsRequired();
        }
    }
}
