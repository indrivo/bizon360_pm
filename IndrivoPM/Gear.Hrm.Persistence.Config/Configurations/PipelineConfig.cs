using Gear.Domain.HrmEntities.Recruitment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Hrm.Persistence.Config.Configurations
{
    public class RecruitmentPipelineConfig : BaseModelConfig<RecruitmentPipeline>
    {
        public override void Configure(EntityTypeBuilder<RecruitmentPipeline> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Description)
                .HasMaxLength(250);

            builder.HasMany(rs => rs.RecruitmentStages)
                .WithOne(rp => rp.Pipeline);

            builder.Metadata.FindNavigation(nameof(RecruitmentPipeline.RecruitmentStages))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
