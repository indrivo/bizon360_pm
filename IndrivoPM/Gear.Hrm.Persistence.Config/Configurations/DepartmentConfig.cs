using Gear.Domain.HrmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Hrm.Persistence.Config.Configurations
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(e => e.Description)
                .HasMaxLength(900);

            builder.HasOne(x => x.DepartmentLead)
                .WithMany(x => x.Department)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.DepartmentTeams)
                .WithOne(x => x.Department)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}