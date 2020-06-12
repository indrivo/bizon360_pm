using Gear.Domain.HrmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Hrm.Persistence.Config.Configurations
{
    public class UserDepartmentTeamConfig : IEntityTypeConfiguration<UserDepartmentTeam>
    {
        public void Configure(EntityTypeBuilder<UserDepartmentTeam> builder)
        {
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.DeparmentTeamId)
                .IsRequired();

            builder.HasKey(x => new { x.DeparmentTeamId, x.UserId });

            builder.HasOne(x => x.DepartmentTeam)
                .WithMany(x => x.UserDepartmentTeams)
                .HasForeignKey(x => x.DeparmentTeamId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserDepartmentTeams)
                .HasForeignKey(x => x.UserId);
        }
    }
}
