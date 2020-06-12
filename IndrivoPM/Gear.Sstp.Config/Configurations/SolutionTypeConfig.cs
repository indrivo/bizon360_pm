using Gear.Sstp.Abstractions.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Sstp.Config.Configurations
{
    public class SolutionTypeConfig : SstpBaseModelConfig<SolutionType>
    {
        public override void Configure(EntityTypeBuilder<SolutionType> builder)
        {
            base.Configure(builder);

        }
    }
}
