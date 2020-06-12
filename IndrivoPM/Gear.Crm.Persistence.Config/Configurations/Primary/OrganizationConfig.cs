using Gear.Domain.CrmEntities.Primary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Crm.Persistence.Config.Configurations.Primary
{
    public class OrganizationConfig : BaseModelConfig<ClientOrganization>
    {
        public override void Configure(EntityTypeBuilder<ClientOrganization> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Contacts)
                .HasField("_contacts")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.Deals)
                .HasField("_deals")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(x => x.Address);

            builder.OwnsOne(x => x.ContactInfo);

        }
    }
}
