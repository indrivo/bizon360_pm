using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class ChangeRequestConfig : BaseModelConfig<ChangeRequest>
    {
        public override void Configure(EntityTypeBuilder<ChangeRequest> builder)
        {
            base.Configure(builder);

            builder.Property(cr => cr.Description)
                .IsRequired(false);

            builder.Property(c => c.Comment)
                .IsRequired(false);

            builder.Property(cr => cr.Status)
                .IsRequired()
                .HasDefaultValue(ChangeRequestStatus.Unprocessed);

            builder.Property(e => e.Number)
                .HasDefaultValueSql("nextval('\"ReferenceSequenceChangeRequests\"')");
        }
    }
}
