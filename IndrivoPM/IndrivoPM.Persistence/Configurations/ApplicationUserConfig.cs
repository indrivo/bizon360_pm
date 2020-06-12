using Gear.Domain.AppEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Persistence.Configurations
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(x => x.JobPosition)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.JobPositionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.DepartmentTeam)
                .WithOne(x => x.DepartmentTeamLead)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Department)
                .WithOne(x => x.DepartmentLead)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.LoggedTime)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.UserDepartmentTeams)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
