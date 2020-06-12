using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class ProjectTeamConfig : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.Property(e => e.ProjectId)
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasKey(t => new { t.ProjectId, t.UserId });

            builder.HasOne(t => t.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(t => t.ProjectId);

            builder.HasOne(t => t.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(t => t.UserId);
        }
    }
}