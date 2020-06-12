using Gear.Sstp.Abstractions.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Sstp.Config.Configurations
{
    public class TechnologyTypeConfig : SstpBaseModelConfig<TechnologyType>
    {
        public override void Configure(EntityTypeBuilder<TechnologyType> builder)
        {
            base.Configure(builder);

        }
    }
}
