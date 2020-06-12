using Gear.Domain.CrmEntities.Primary;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Crm.Persistence.Config.Configurations.Primary
{
    public class PipelineConfig : BaseModelConfig<DealPipeline>
    {
        public override void Configure(EntityTypeBuilder<DealPipeline> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}
