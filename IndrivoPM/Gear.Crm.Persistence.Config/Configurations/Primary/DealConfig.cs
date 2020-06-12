using Gear.Domain.CrmEntities.Primary;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Crm.Persistence.Config.Configurations.Primary
{
    public class DealConfig : BaseModelConfig<Deal>
    {
        public override void Configure(EntityTypeBuilder<Deal> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.DealValue)
                .IsRequired();

            builder.Property(x => x.DealStatus)
                .IsRequired(); ;

            builder.Property(x => x.DealStageId)
                .IsRequired();

            builder.Property(x => x.OrganizationId)
                .IsRequired();

            builder.Property(x => x.ApplicationUserId)
                .IsRequired();

            builder.Property(x => x.CurrencyId)
                .IsRequired();

            builder.Property(x => x.Region)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}
