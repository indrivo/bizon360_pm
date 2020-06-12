using Gear.Domain.PmEntities.Wiki;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class SectionConfig : BaseModelConfig<Section>
    {
        public override void Configure(EntityTypeBuilder<Section> builder)
        {
            base.Configure(builder);
        }
    }
}
