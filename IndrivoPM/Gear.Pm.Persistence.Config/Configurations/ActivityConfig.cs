using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class ActivityConfig : BaseModelConfig<Activity>
    {
        public override void Configure(EntityTypeBuilder<Activity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ActivityPriority)
                .IsRequired();

            builder.Property(e => e.ActivityStatus)
                .IsRequired();

            builder.Property(a => a.Progress)
                .IsRequired();

            builder.Property(a => a.EstimatedHours)
                .IsRequired(false);

            builder.Property(a => a.StartDate)
                .IsRequired(false);

            builder.Property(a => a.DueDate)
                .IsRequired(false);

            ////builder.HasOne(x => x.Author)
            ////    .WithMany()
            ////    .HasForeignKey(x => x.CreatedBy);
            //builder.Ignore(x => x.Author);

            builder.Property(e => e.Number)
                .HasDefaultValueSql("nextval('\"ReferenceSequenceActivities\"')");
        }
    }
}
