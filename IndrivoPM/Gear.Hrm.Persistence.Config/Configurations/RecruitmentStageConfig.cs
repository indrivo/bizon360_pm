using Gear.Domain.HrmEntities.Recruitment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Hrm.Persistence.Config.Configurations
{
    public class RecruitmentStageConfig : BaseModelConfig<RecruitmentStage>
    {
        public override void Configure(EntityTypeBuilder<RecruitmentStage> builder)
        {
            base.Configure(builder);

            builder.HasMany(c => c.Candidates)
                .WithOne(rs => rs.RecruitmentStage);

            builder.Metadata.FindNavigation(nameof(RecruitmentStage.Candidates))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
