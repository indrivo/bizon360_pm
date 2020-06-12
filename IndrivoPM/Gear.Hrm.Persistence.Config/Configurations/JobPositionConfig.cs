using Gear.Domain.HrmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Hrm.Persistence.Config.Configurations
{
    public class JobPositionConfig: BaseModelConfig<JobPosition>
    {
        public override void Configure(EntityTypeBuilder<JobPosition> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasMany(x => x.JobDepartmentTeams)
                .WithOne(x => x.JobPosition)
                .OnDelete(DeleteBehavior.Cascade);

/*            builder.HasMany(x => x.Candidates)
                .WithOne(x => x.JobPosition)
                .HasForeignKey(x => x.JobPositionId)
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}
