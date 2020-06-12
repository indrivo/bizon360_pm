using Gear.Domain.PmEntities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations.Settings
{
    public class ProjectSettingsConfig : BaseModelConfig<ProjectSettings>
    {
        public override void Configure(EntityTypeBuilder<ProjectSettings> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ProjectDashboardTab)
                .HasDefaultValue(false);

            builder.Property(x => x.ActivityTypesTab)
                .HasDefaultValue(true);
        }
    }
}
