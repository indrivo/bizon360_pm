using Gear.Identity.Permissions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Identity.Permissions.Config.Configs
{
    public class RoleToPermissionsConfiguration : IEntityTypeConfiguration<RoleToPermissions>
    {
        public void Configure(EntityTypeBuilder<RoleToPermissions> builder)
        {
            builder.HasKey(x => x.RoleName);

            builder.Property(x => x.RoleName)
                .IsRequired()
                .HasMaxLength(450)
                .ValueGeneratedNever();

            builder.Property(x => x.Description)
                .IsRequired();

        }
    }
}
