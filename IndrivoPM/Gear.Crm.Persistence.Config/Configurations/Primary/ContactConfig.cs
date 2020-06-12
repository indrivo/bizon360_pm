using Gear.Domain.CrmEntities.Primary;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Crm.Persistence.Config.Configurations.Primary
{
    public class ContactConfig : BaseModelConfig<ClientContact>
    {
        public override void Configure(EntityTypeBuilder<ClientContact> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.OrganizationId)
                .IsRequired();

            builder.OwnsOne(x => x.ContactInfo);

        }
    }
}
