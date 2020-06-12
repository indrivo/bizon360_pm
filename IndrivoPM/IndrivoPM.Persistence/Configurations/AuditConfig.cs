using Gear.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Persistence.Configurations
{
    public class AuditConfig : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(x => x.Id)               
                .IsRequired();

            builder.Property(x => x.AuditData);

            builder.Property(x => x.AuditDate)
                .IsRequired();

            builder.Property(x => x.AuditUserName)
                .IsRequired();

            builder.Property(x => x.AuditUserId)
                .IsRequired();

            builder.Property(x => x.EntityPk)
                .IsRequired();

            builder.Property(x => x.TableName)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired();

            builder.HasMany(x => x.Changes)
                .WithOne(x => x.AuditLog);

            builder.Metadata.FindNavigation(nameof(AuditLog.Changes))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class EntryChangeConfig : IEntityTypeConfiguration<EntryChange>
    {
        public void Configure(EntityTypeBuilder<EntryChange> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.AuditLogId)
                .IsRequired();
        }
    }
}
