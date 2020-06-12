using Gear.Sstp.Abstractions.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Sstp.Config.Configurations
{
    public class ProductTypeConfig:SstpBaseModelConfig<ProductType>
    {
        public override void Configure(EntityTypeBuilder<ProductType> builder)
        {
            base.Configure(builder);

        }
    }
}
