using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Domain.PmEntities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class ProjectConfig : BaseModelConfig<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.Property(e => e.ProjectUrl)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(e => e.StartDate)
                .IsRequired(false);

            builder.Property(e => e.EndDate)
                .IsRequired(false);

            builder.Property(e => e.Budget)
                .IsRequired(false);

            builder.Property(e => e.ProjectManagerId)
                .IsRequired(false);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValue(ProjectStatus.New);

            builder.Property(e => e.Priority)
                .IsRequired()
                .HasDefaultValue(ActivityPriority.Low);

            builder.HasMany(p => p.ActivityLists)
                .WithOne(al => al.Project)
                .HasForeignKey(al => al.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Sprints)
                .WithOne(s => s.Project)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.ProjectSettings)
                .WithOne(ps => ps.Project)
                .HasForeignKey<ProjectSettings>(ps => ps.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.ProjectInvoice)
                .WithOne(pI => pI.Project)
                .HasForeignKey<ProjectInvoice>(pi => pi.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Activities)
                .WithOne(a => a.Project)
                .HasForeignKey(a => a.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ChangeRequests)
                .WithOne(cr => cr.Project)
                .HasForeignKey(cr => cr.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.WikiHeadlines)
                .WithOne(h => h.Project)
                .HasForeignKey(h => h.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Number)
                .HasDefaultValueSql("nextval('\"ReferenceSequenceProjects\"')");
        }
    }
}
