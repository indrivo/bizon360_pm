using Gear.Domain.CrmEntities.Secondary;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Crm.Persistence.Config.Configurations.Secondary
{
    public class CurrencyConfig: BaseModelConfig<Currency>
    {
        public override void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.Property(x => x.Sign)
                .IsRequired();
        }
    }
}
