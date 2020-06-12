using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class ProjectGroupConfig : BaseModelConfig<ProjectGroup>
    {
        public override void Configure(EntityTypeBuilder<ProjectGroup> builder)
        {
            base.Configure(builder);

            builder.HasMany(pg => pg.Projects)
                .WithOne(p => p.ProjectGroup)
                .HasForeignKey(p => p.ProjectGroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
