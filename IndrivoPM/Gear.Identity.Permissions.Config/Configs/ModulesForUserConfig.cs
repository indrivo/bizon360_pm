using Gear.Identity.Permissions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Identity.Permissions.Config.Configs
{
    public class ModulesForUserConfig : IEntityTypeConfiguration<ModulesForUser>
    {
        public void Configure(EntityTypeBuilder<ModulesForUser> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId)
                .IsRequired()
                .ValueGeneratedNever()
                .HasMaxLength(450);

        }
    }
}
