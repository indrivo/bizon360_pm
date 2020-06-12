using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class ActivityAssigneeConfig : IEntityTypeConfiguration<ActivityAssignee>
    {
        public void Configure(EntityTypeBuilder<ActivityAssignee> builder)
        {
            builder.Property(e => e.ActivityId)
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasKey(e => new { e.ActivityId, e.UserId });

            builder.HasOne(aa => aa.Activity)
                .WithMany(a => a.Assignees)
                .HasForeignKey(aa => aa.ActivityId);

            builder.HasOne(aa => aa.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(aa => aa.UserId);
        }
    }
}
