using Gear.Domain.CrmEntities.Primary;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Crm.Persistence.Config.Configurations.Primary
{
    public class DealStageConfig : BaseModelConfig<DealStage>
    {
        public override void Configure(EntityTypeBuilder<DealStage> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.OrderInsidePipeline)
                .IsRequired();

            builder.Property(x => x.PipelineId)
                .IsRequired();
        }
    }
}
