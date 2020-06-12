using Gear.Domain.CrmEntities.ManyToManyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Crm.Persistence.Config.Configurations.ManyToManyEntities
{
    public class DealContactConfig : IEntityTypeConfiguration<DealContact>
    {
        public void Configure(EntityTypeBuilder<DealContact> builder)
        {
            builder.Property(x => x.ContactId)
                .IsRequired();

            builder.Property(x => x.DealId)
                .IsRequired();

            builder
                .HasKey(dc => new { dc.DealId, dc.ContactId });

            //TODO: Check to Remove this in the future as they are not really required
            //builder
            //    .HasOne(dc => dc.Deal)
            //    .WithMany(c => c.Contacts)
            //    .HasForeignKey(d => d.DealId);

            //builder
            //    .HasOne(dc => dc.ClientContact)
            //    .WithMany(d => d.Deals)
            //    .HasForeignKey(dc => dc.ContactId);
        }
    }
}
