using Gear.Sstp.Abstractions.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Sstp.Config.Configurations
{
    public class ServiceTypeConfig:SstpBaseModelConfig<ServiceType>
    {
        public override void Configure(EntityTypeBuilder<ServiceType> builder)
        {
            base.Configure(builder);

        }
    }
}
