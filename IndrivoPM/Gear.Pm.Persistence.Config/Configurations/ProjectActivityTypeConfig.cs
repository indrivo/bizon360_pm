using System;
using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class ProjectActivityTypeConfig : IEntityTypeConfiguration<ProjectActivityType>
    {
        public void Configure(EntityTypeBuilder<ProjectActivityType> builder)
        {
            builder.HasKey(x => new {x.ProjectId, x.ActivityTypeId});

            builder.HasOne(x => x.Project)
                .WithMany(x => x.ProjectActivityTypes)
                .HasForeignKey(x => x.ProjectId);

            builder.HasOne(x => x.ActivityType)
                .WithMany(x => x.ProjectActivityTypes)
                .HasForeignKey(x => x.ActivityTypeId);

            builder.Property(x => x.Active)
                .HasDefaultValue(true);
        }
    }
}
