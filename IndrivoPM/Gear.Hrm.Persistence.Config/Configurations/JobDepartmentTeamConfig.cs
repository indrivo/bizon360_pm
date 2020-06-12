using Gear.Domain.HrmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Hrm.Persistence.Config.Configurations
{
    public class JobDepartmentTeamConfig : IEntityTypeConfiguration<JobDepartmentTeam>
    {
        public void Configure(EntityTypeBuilder<JobDepartmentTeam> builder)
        {
            builder.Property(x => x.JobPositionId)
                .IsRequired();

            builder.Property(x => x.DepartmentTeamId)
                .IsRequired();

            builder.HasKey(x => new { x.JobPositionId, x.DepartmentTeamId });

            builder.HasOne(x => x.DepartmentTeam)
                .WithMany(x => x.JobDepartmentTeams)
                .HasForeignKey(x => x.DepartmentTeamId);

            builder.HasOne(x => x.JobPosition)
                .WithMany(x => x.JobDepartmentTeams)
                .HasForeignKey(x => x.JobPositionId);
        }
    }
}
