using Gear.Domain.HrmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Hrm.Persistence.Config.Configurations
{
    public class DepartmentTeamConfig : BaseModelConfig<DepartmentTeam>
    {
        public override void Configure(EntityTypeBuilder<DepartmentTeam> builder)
        {
            builder.Property(e => e.Description)
                .HasMaxLength(900);

            builder.Property(x => x.Active).HasDefaultValue(true);

            builder.HasMany(x => x.JobDepartmentTeams)
                .WithOne(x => x.DepartmentTeam)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.UserDepartmentTeams)
                .WithOne(x => x.DepartmentTeam)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.DepartmentTeamLead)
                .WithMany(x => x.DepartmentTeam)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}